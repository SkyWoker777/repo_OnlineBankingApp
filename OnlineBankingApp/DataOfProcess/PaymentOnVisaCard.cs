using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.DataOfProcess
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PaymentOnVisaCard : Payment
    {
        [JsonProperty(PropertyName = "recipient_name")]
        public string Name { get; set; }
    }
}
