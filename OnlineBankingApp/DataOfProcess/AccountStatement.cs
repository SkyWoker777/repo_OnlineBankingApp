using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OnlineBankingApp.DataOfProcess
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AccountStatement
    {
        [JsonProperty(PropertyName = "card")]
        public string Card { get; set; }

        [JsonProperty(PropertyName = "from_date")]
        public string FromDate { get; set; }

        [JsonProperty(PropertyName = "payment_id")]
        public string PaymentId { get; set; }

        [JsonProperty(PropertyName = "to_date")]
        public string ToDate { get; set; }

    }
}
