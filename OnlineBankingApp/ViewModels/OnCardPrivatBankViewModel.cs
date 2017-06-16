using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBankingApp.DataOfProcess;
using OnlineBankingApp.Models;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OnlineBankingApp.ViewModels
{
    public class OnCardPrivatBankViewModel : BaseViewModel
    {
        private string amount;
        private string currency;
        private string recipient;
        private string description;
        private string card_sender;
        private double valueBar = 0;
        private bool flag = false;
        private Visibility _visibility = Visibility.Collapsed;
        private string percent = "0%";
        private string sectionDb = "on_card_privatbank";

        DispatcherTimer timer;

        #region BindingProperties
        public ObservableCollection<ResponseAccountBalance> CardsList { get; set; }
        public string Amt
        {
            get { return amount; }
            set { amount = value; }
        }
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        public string Recipient
        {
            get { return recipient; }
            set
            {
                recipient = value;
                RaisePropertyChanged("Recipient");
            }
        }
        public string CardSender
        {
            get { return card_sender; }
            set
            {
                card_sender = value;
                RaisePropertyChanged("CardSender");
            }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool IsActiveProperty
        {
            get { return flag; }
            set
            {
                flag = value;
                RaisePropertyChanged("IsActiveProperty");
            }
        }
        public string TextProperty
        {
            get { return percent; }
            set
            {
                percent = value;
                RaisePropertyChanged("TextProperty");
            }
        }
        public Visibility VisionProperty
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                RaisePropertyChanged("VisionProperty");
            }
        }
        #endregion

        public ICommand TransferCommand => new DelegateCommand(o => TransferFunds());

        public OnCardPrivatBankViewModel()
        {
            CardsList = new ObservableCollection<ResponseAccountBalance>();
            GetCardsWithBalanceFromDb();
        }

        private async void TransferFunds()
        {
            if (!String.IsNullOrWhiteSpace(amount) && !String.IsNullOrWhiteSpace(recipient) &&
                !String.IsNullOrWhiteSpace(description) && !String.IsNullOrWhiteSpace(card_sender))
            {
                Task<ResponsePayment> paymentTask = SendPaymentAsync();
                //progress bar TODO
                //data processing...
                //
                ResponsePayment payment = await paymentTask;
                string message = string.Empty;
                if (payment != null)
                {
                    message = "Платеж был отправлен.\n"
                        + "Cумма: " + payment.Amount +" "+ payment.Currency 
                        +"\nКомиссия: " + payment.Comis + "\n\n" + "Подтверждение платежа не требуется.";
                    var result = await ContentSuccess.ShowAsync(message);
                    if (result == ContentDialogResult.Primary)
                    {
                        //ArrayList parameters = new ArrayList();
                        //parameters.Add(payment);
                        //parameters.Add(sectionDb);
                        NavigationService.Instance.NavigateTo(typeof(HomePage));
                    }
                }
            }
            else
            {
                ContentError.Show("Заполнены не все поля.");
            }
        }

        //Payment operations
        #region SendPayment
        private async Task<string> PaymentCreatingAsync()
        {
            Payment payment = new Payment()
            {
                Amount = amount,
                Currency = currency,
                Description = description,
                PaymentId = Util.GetPaymentId(),
                Recipient = recipient,
            };
            JObject data = JObject.FromObject(payment);
            data.Add("merchant", User.MerchId);
            Operation operation = new Operation()
            {
                Ref = Util.GetRef(),
                Type = "create",
                Obj = "task",
                Conv_Id = "206043",
                Data = data,
            };

            CorezoidMessage message = new CorezoidMessage();
            message.Create(operation, Util.ApiLogin, Util.ApiSecret);
            string json = await message.SendAsync();
            return json;
        }
        private async Task<ResponsePayment> SendPaymentAsync()
        {
            Task<string> taskPayment = PaymentCreatingAsync();
            ResponsePayment resp_payment = new ResponsePayment();
            string response = await taskPayment;
            if (ResponseOperation.GetStatusProc(response) == "ok")
            {
                FirebaseManager fm = new FirebaseManager("payments", User.MerchId);
                try
                {
                    OnLaunchTimerProgressBar();
                    await Task.Delay(15000);
                    Task<string> contentTask = await taskPayment.ContinueWith(t => fm.GetContentAsync(sectionDb));
                    string content = await contentTask;
                    JObject obj = JObject.Parse(content);
                    IEnumerable<ResponsePayment> pays = JsonConvert.DeserializeObject<IEnumerable<ResponsePayment>>(
                        obj["pays"].ToString()
                        );
                    foreach (var item in pays)
                    {
                        resp_payment = item;
                    }
                }
                catch(JsonReaderException ex){
                    System.Diagnostics.Debug.WriteLine("JException: " + ex.Message);
                    ContentError.Show("Не удалось получить информацию о платеже.");
                }
                catch(NullReferenceException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                return resp_payment;
            }
            else
            {
                ContentError.Show("Ошибка выполнения процесса! Операция отклонена.");
                return null;
            }
        }

        #endregion

        private void Timer_Tick(object sender, object e)
        {
            TextProperty = (valueBar++).ToString() + "%";
            if (valueBar == 100)
            {
                timer.Stop();
                IsActiveProperty = false;
                VisionProperty = Visibility.Collapsed;
                Window.Current.CoreWindow.IsInputEnabled = true;
            }
        }
        private void OnLaunchTimerProgressBar()
        {
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 150) };
            IsActiveProperty = true;
            VisionProperty = Visibility.Visible;
            Window.Current.CoreWindow.IsInputEnabled = false;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void GetCardsWithBalanceFromDb()
        {
            FirebaseManager fm = new FirebaseManager("acc_balance", User.MerchId);
            for (int i = 0; i < User.Cards.Count; i++)
            {
                string section = User.Cards.ElementAt(i);
                try
                {
                    string content = await fm.GetContentAsync(section);
                    JObject obj = JObject.Parse(content);
                    ResponseAccountBalance info = JsonConvert.DeserializeObject<ResponseAccountBalance>(obj["info"].ToString());
                    if (info.Currency == "UAH") { info.Currency = "грн"; }
                    else if (info.Currency == "USD") { info.Currency = "дол"; }
                    else { info.Currency = "евро"; }
                    CardsList.Add(info);
                }
                catch(JsonReaderException ex)
                {
                    System.Diagnostics.Debug.WriteLine("JException: " + ex.Message);
                }
                catch (AggregateException exp)
                {
                    System.Diagnostics.Debug.WriteLine("AggreException: " + exp.InnerException.Message);
                }
            }
        }

    }
}
