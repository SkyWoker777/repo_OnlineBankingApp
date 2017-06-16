using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OnlineBankingApp.ViewModels
{
    public class ExchangeRatesViewModel : BaseViewModel
    {
        private ResponseExchangeRates rates;
        //Binding Properties
        public ResponseExchangeRates RatesProperty
        {
            get { return rates; }
            set
            {
                rates = value;
                RaisePropertyChanged("RatesProperty");
            }
        }
        public string Today
        {
            get { return DateTime.Today.ToString("dd.MM.yyyy"); } 
        }
        public string TextCh { get; set; }
        public string TextGetting { get; set; }
        //------------
        public ExchangeRatesViewModel()
        {
            GetRates();
        }

        private async void GetRates()
        {
            RequestRates();
            try
            {
                FirebaseManager manager = new FirebaseManager("exchange_rates");
                string content = await manager.GetContentAsync();
                JObject obj = JObject.Parse(content);
                RatesProperty = JsonConvert.DeserializeObject<ResponseExchangeRates>(obj.ToString());
            }
            catch (JsonReaderException) { ContentError.Show("Не удалось загрузить данные."); }
        }

        private async void RequestRates()
        {
            Operation operation = new Operation()
            {
                Ref = Util.GetRef(),
                Type = "create",
                Obj = "task",
                Conv_Id = "213697",
                Data = new JObject(),
            };

            CorezoidMessage message = new CorezoidMessage();
            message.Create(operation, Util.ApiLogin, Util.ApiSecret);
            await message.SendAsync();
        }
    }
}
