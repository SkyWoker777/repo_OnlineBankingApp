using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.DataOfProcess
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PaymentStatus
    {
        [JsonProperty(PropertyName = "ref")]
        public string Ref { get; set; }

        [JsonProperty(PropertyName = "payment_id")]
        public string Id { get; set; }
    }
}
