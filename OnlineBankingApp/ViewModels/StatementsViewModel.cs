using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBankingApp.DataOfProcess;
using OnlineBankingApp.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OnlineBankingApp.ViewModels
{
    public class StatementsViewModel : BaseViewModel
    {
        private static string fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd.MM.yyyy");
        private static string toDate = DateTime.Today.ToString("dd.MM.yyyy");
        private bool flag = false;

        public ObservableCollection<Statement> StatementList { get; set; }
        public bool IsActiveProperty
        {
            get { return flag; }
            set
            {
                flag = value;
                RaisePropertyChanged("IsActiveProperty");
            }
        }

        public StatementsViewModel() { }
        public StatementsViewModel(string cardNumber)
        {
            GetStatements(cardNumber);
        }

        private async Task<string> SendMessageAsync(string cardNumber)
        {
            AccountStatement statement = new AccountStatement()
            {
                Card = cardNumber,
                FromDate = fromDate,
                PaymentId = Util.GetPaymentId(),
                ToDate = toDate,
            };

            JObject data = JObject.FromObject(statement);
            data.Add("merchant", User.MerchId);
            Operation operation = new Operation()
            {
                Ref = Util.GetRef(),
                Type = "create",
                Obj = "task",
                Conv_Id = "207804",
                Data = data,
            };

            CorezoidMessage message = new CorezoidMessage();
            message.Create(operation, Util.ApiLogin, Util.ApiSecret);
            string json = await message.SendAsync();
            return json;
        }

        private async void GetStatements(string cardNumber)
        {
            StatementList = new ObservableCollection<Statement>();
            string response = await SendMessageAsync(cardNumber);
            if (ResponseOperation.GetStatusProc(response) == "ok")
            {
                FirebaseManager fm = new FirebaseManager("statements", User.MerchId);
                try
                {
                    IsActiveProperty = true;
                    await Task.Delay(2000);
                    string content = await fm.GetContentAsync(cardNumber);
                    JObject obj = JObject.Parse(content);
                    ResponseStatements result = JsonConvert.DeserializeObject<ResponseStatements>(obj["result"].ToString());
                    if (result.Statements != null)
                    {
                        foreach (var item in result.Statements)
                        {
                            item.Details = ((WebUtility.HtmlDecode(item.Details).Replace("&quot;", @"""")));
                            StatementList.Add(item);
                        }
                    }
                }
                catch(JsonReaderException) {
                    ContentError.Show("Не удалось загрузить данные.");
                }
                catch(AggregateException exp)
                {
                    System.Diagnostics.Debug.WriteLine("Error Info: " + exp.InnerException.Message); 
                }
                IsActiveProperty = false;
                RaisePropertyChanged("StatementList");
            }
            else
            {
                ContentError.Show("Ошибка чтения данных процесса!");
            }
        }

    }
}
