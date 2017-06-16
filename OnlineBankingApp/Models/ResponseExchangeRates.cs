using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResponseExchangeRates
    {
        [JsonProperty(PropertyName = "buy_EUR")]
        public string BuyEUR { get; set; }

        [JsonProperty(PropertyName = "buy_USD")]
        public string BuyUSD { get; set; }

        [JsonProperty(PropertyName = "buy_RUR")]
        public string BuyRUR { get; set; }

        [JsonProperty(PropertyName = "sale_EUR")]
        public string SaleEUR { get; set; }

        [JsonProperty(PropertyName = "sale_USD")]
        public string SaleUSD { get; set; }

        [JsonProperty(PropertyName = "sale_RUR")]
        public string SaleRUR { get; set; }
    }
}
