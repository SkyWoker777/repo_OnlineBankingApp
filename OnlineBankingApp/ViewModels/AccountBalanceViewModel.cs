using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBankingApp.DataOfProcess;
using OnlineBankingApp.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnlineBankingApp.ViewModels
{
    public class AccountBalanceViewModel : BaseViewModel
    {
        private bool flag = false;
        public ObservableCollection<ResponseAccountBalance> CardsList { get; set; }
        public bool IsActiveProperty
        {
            get { return flag; }
            set
            {
                flag = value;
                RaisePropertyChanged("IsActiveProperty");
            }
        }
        public AccountBalanceViewModel()
        {
            GetCards();
        }

        public ICommand UpdateCommand => new DelegateCommand(o => GetCards());

        private async void GetCards()
        {
            CardsList = new ObservableCollection<ResponseAccountBalance>();
            //string responseInfo = await SendMessageToCorezoid();
            //if (ResponseOperation.GetStatusProc(responseInfo) == "ok")
            //{
                FirebaseManager fm = new FirebaseManager("acc_balance", User.MerchId);
                //IsActiveProperty = true;
                //await Task.Delay(1500);
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
                    catch (JsonReaderException)
                    {
                        ContentError.Show("Ошибка загрузки данных! Пожалуйста, проверьте Ваше подключение к Интернет.");
                    }
                    catch (NullReferenceException ex)
                    {
                        System.Diagnostics.Debug.WriteLine("NRExp: " + ex.Message);
                    }
                }
            //    IsActiveProperty = false;
            //    RaisePropertyChanged("CardsList");
            //}
            //else
            //{
            //    ContentError.Show("Ошибка чтения данных процесса!");
            //}
        }

        private async Task<List<Operation>> CreateListOps()
        {
            List<Operation> operations = new List<Operation>();
            
            for (int i = 0; i < User.Cards.Count; i++)
            {
                AccountBalance balance = new AccountBalance()
                {
                    Card = User.Cards.ElementAt(i),
                    Country = "UA",
                    PaymentId = Util.GetPaymentId(),
                };
                JObject data = JObject.FromObject(balance);
                data.Add("merchant", User.MerchId);
                await Task.Delay(322);
                Operation operation = new Operation()
                {
                    Ref = Util.GetRef(),
                    Type = "create",
                    Obj = "task",
                    Conv_Id = "206342",
                    Data = data,
                };
                operations.Add(operation);
            }
            return operations;
        }
        private async Task<string> SendMessageToCorezoid()
        {
            List<Operation> operations = await CreateListOps();
            CorezoidMessage message = new CorezoidMessage();
            message.Create(operations, Util.ApiLogin, Util.ApiSecret);
            string json = await message.SendAsync();
            return json;
        }
    }
}
