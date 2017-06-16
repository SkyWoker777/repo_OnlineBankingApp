using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResponseStatements
    {
        [JsonProperty(PropertyName = "credit")]
        public string Credit { get; set; }

        [JsonProperty(PropertyName = "debet")]
        public string Debit { get; set; }

        [JsonProperty(PropertyName = "statements")]
        public IEnumerable<Statement> Statements { get; set; }
    }
}
