using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResponsePayment
    {
        [JsonProperty(PropertyName = "amt")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "cardinfo")]
        public string CardInfo { get; set; }

        [JsonProperty(PropertyName = "ccy")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "comis")]
        public string Comis { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string PaymentId { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public string RefPrivat { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }
}
