using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResponseMobileRefill
    {
        [JsonProperty(PropertyName = "commission")]
        public string Commission { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public string RefPivat { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }
}
