using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp
{
    [JsonObject(MemberSerialization.OptIn)]
    class Session
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

        [JsonProperty(PropertyName = "expiresIn")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "roles")]
        public IEnumerable<string> Roles { get; set; }
    }
}
