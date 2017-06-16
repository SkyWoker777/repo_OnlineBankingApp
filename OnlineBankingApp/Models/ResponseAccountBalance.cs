using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResponseAccountBalance
    {
        [JsonProperty(PropertyName = "balance")]
        public string Balance { get; set; }

        [JsonProperty(PropertyName = "card")]
        public string Card { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "finLimit")]
        public string FinLimit { get; set; }

        [JsonProperty(PropertyName = "tradeLimit")]
        public string TradeLimit { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

    }
}
