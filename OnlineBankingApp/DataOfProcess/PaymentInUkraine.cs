using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.DataOfProcess
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PaymentInUkraine : Payment
    {
        /// <summary>
        /// р/с получателя или карта
        /// </summary>
        [JsonProperty(PropertyName = "recipient_acc")]
        public string Acc { get; set; }

        [JsonProperty(PropertyName = "recipient_mfo")]
        public string MFO { get; set; }

        [JsonProperty(PropertyName = "recipient_okpo")]
        public string OKPO { get; set; }

        [JsonProperty(PropertyName = "recipient_name")]
        public string Name { get; set; }
    }
}
