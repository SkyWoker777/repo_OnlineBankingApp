using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.DataOfProcess
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Payment
    {
        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// recipient Card Number
        /// </summary>
        [JsonProperty(PropertyName = "recipient")]
        public string Recipient { get; set; }
        /// <summary>
        /// Id платежа, указанный мерчантом
        /// </summary>
        [JsonProperty(PropertyName = "payment_id")]
        public string PaymentId { get; set; }
    }
}
