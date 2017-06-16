using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnlineBankingApp.ViewModels
{
    public class PaymentStatusViewModel : BaseViewModel
    {
        private string section;
        private bool flag = false;
        ResponsePayment resp_payment;
        //convId: "213390" on cardPB
        //convId: "213563" on visa any bank

        public PaymentStatusViewModel() { }
        public PaymentStatusViewModel(ResponsePayment rp ,string _sectionDb)
        {
            resp_payment = rp;
            section = _sectionDb;
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
        public ICommand ShowCommand => new DelegateCommand(o => ShowStatusClick());
        private async void ShowStatusClick()
        {
            string message = await GetStatusAsync();
            if (!String.IsNullOrWhiteSpace(message))
            {
            }
        }

        private async Task<string> GetStatusAsync()
        {
            string message = string.Empty;
            if (resp_payment.State == "1")
            {
                ResponseStatus response = new ResponseStatus();
                response.RequestPaymentStatus(resp_payment.PaymentId, resp_payment.RefPrivat, User.MerchId, convId: "213390");
                FirebaseManager fm = new FirebaseManager("payments", User.MerchId);
                try
                {
                    IsActiveProperty = true;
                    await Task.Delay(1000);
                    string content = await fm.GetContentAsync(section);
                    JObject obj = JObject.Parse(content);
                    response = JsonConvert.DeserializeObject<ResponseStatus>(
                        obj["status"].ToString()
                        );
                }
                catch (AggregateException exp)
                {
                    ContentError.Show("Ошибка загрузки данных.");
                    System.Diagnostics.Debug.WriteLine("Error: " + exp.InnerException.Message);
                }
                switch (response.Status)
                {
                    case "not found":
                        message = "Платеж не найден!";
                        break;
                    case "ok":
                        message = "Платеж успешно проведен!";
                        break;
                    case "snd":
                        message =
                            "Находится в обработке. Платеж будет осуществлен в течении одного банковского дня. Ожидайте...";
                        break;
                }
                IsActiveProperty = false;
                return message;
            }
            else
            {
                ContentError.Show("Платеж был отклонен или заблокирован.");
                return null;
            }
        }
    }
}
