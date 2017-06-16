using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBankingApp.DataOfProcess;
using OnlineBankingApp.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnlineBankingApp.ViewModels
{
    public class MobileRefillViewModel : BaseViewModel
    {
        private string phoneNumber;
        private string amount;
        private string mobOperator;
        private bool flag = false;

        #region BindingProperties
        public Dictionary<string, string> operatorDictionary { get; set; }
        public ObservableCollection<ResponseAccountBalance> CardsList { get; set; }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                RaisePropertyChanged("PhoneNumber");
            }
        }
        public string Amt
        {
            get { return amount; }
            set
            {
                amount = value;
                RaisePropertyChanged("Amt");
            }
        }
        public string MobOperator
        {
            get { return mobOperator; }
            set
            {
                mobOperator = value;
                RaisePropertyChanged("MobOperator");
            }
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
        #endregion
        public MobileRefillViewModel()
        {
            operatorDictionary = new Dictionary<string, string>();
            CardsList = new ObservableCollection<ResponseAccountBalance>();
            FillDictionary();
            GetCardsFromDb();
        }

        public ICommand PayMobile => new DelegateCommand(o => OnRefill());

        private void FillDictionary()
        {
            operatorDictionary.Add("Vodafon", "RPMTSU");
            operatorDictionary.Add("Киевстар", "RPKSTR");
            operatorDictionary.Add("Beeline Украина", "RPBLUA");
            operatorDictionary.Add("life:)", "RPLIFE");
            operatorDictionary.Add("PEOPLEnet", "PPNET");
            operatorDictionary.Add("Utel", "RPUTEL");
            operatorDictionary.Add("Интернет Киевстар", "RPKSIN");
        }

        private async void OnRefill()
        {
            ResponseMobileRefill response = await PaymentProcessing();
            if (response != null)
            {
                if (response.State == "1")
                {
                    //TODO: represent
                    double sum = Convert.ToDouble(amount) + Convert.ToDouble(response.Commission.Replace(".", ","));
                    string message = "Операция выполнена. " + response.Message + "\n\n" +
                        "С учетом комиссии: " + sum.ToString() + " " + response.Currency + "\n" +
                        "Комиссия: " + response.Commission + " " + response.Currency;

                    var result = await ContentSuccess.ShowAsync(message);
                    if (result == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
                    {
                        NavigationService.Instance.NavigateTo(typeof(HomePage));
                    } 
                }
                else { ContentError.Show("Операция отклонена! " + response.Message); }
            }
            else { ContentError.Show("Ошибка загрузки данных. Пожалуйста, проверьте Ваше подключение к Интернет."); }
        }

        #region SendPayment
        private async Task<string> SendPaymentAsync()
        {
            var operatorValue = operatorDictionary[mobOperator];
            MobileRefill paymentRefill = new MobileRefill()
            {
                Amount = amount,
                Operator = operatorValue, //e.g. "RPMTSU"
                PaymentId = Util.GetPaymentId(),
                Phone = phoneNumber,
            };

            JObject data = JObject.FromObject(paymentRefill);
            data.Add("merchant", User.MerchId);

            Operation operation = new Operation()
            {
                Ref = Util.GetRef(),
                Type = "create",
                Obj = "task",
                Conv_Id = "213655",
                Data = data,
            };

            CorezoidMessage message = new CorezoidMessage();
            message.Create(operation, Util.ApiLogin, Util.ApiSecret);
            string json = await message.SendAsync();
            return json; 
        }

        private async Task<ResponseMobileRefill> PaymentProcessing()
        {
            string responseInfo = await SendPaymentAsync();
            ResponseMobileRefill mobRefill = new ResponseMobileRefill();
            if (ResponseOperation.GetStatusProc(responseInfo) == "ok")
            {
                FirebaseManager fm = new FirebaseManager("payments", User.MerchId);
                string section = "mobile_refill";
                try
                {
                    IsActiveProperty = true;
                    await Task.Delay(1500);
                    string content = await fm.GetContentAsync(section);
                    JObject obj = JObject.Parse(content);
                    mobRefill = JsonConvert.DeserializeObject<ResponseMobileRefill>(obj.ToString());
                }
                catch (ArgumentNullException) { }
                catch (JsonReaderException e) {
                    System.Diagnostics.Debug.WriteLine("JException: " + e.Message);
                }
                IsActiveProperty = false;
                return mobRefill;
            }
            else
            {
                return null;
            }
        }
        #endregion

        private async void GetCardsFromDb()
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
                catch (JsonReaderException exp)
                {
                    System.Diagnostics.Debug.WriteLine("JsonExp: " + exp.Message);
                }
                catch (NullReferenceException ex)
                {
                    System.Diagnostics.Debug.WriteLine("NRExp: " + ex.Message);
                }
            }
        }
    }
}
