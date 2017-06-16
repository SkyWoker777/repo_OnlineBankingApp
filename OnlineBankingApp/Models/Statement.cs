using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Statement
    {
        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "card")]
        public string Card { get; set; }

        [JsonProperty(PropertyName = "card_amount")]
        public string CardAmount { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "details")]
        public string Details { get; set; }

        public override string ToString()
        {
            return CardAmount + " " + Currency +
                "\n" + Date + "\n" + Details; 
        }
    }
}
