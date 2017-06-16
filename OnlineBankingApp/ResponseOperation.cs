using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp
{
    [JsonObject(MemberSerialization.OptIn)]
   public class ResponseOperation
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "proc")]
        public string Proc { get; set; }

        [JsonProperty(PropertyName = "obj_id")]
        public string ObjId { get; set; }

        [JsonProperty(PropertyName = "obj")]
        public string Obj { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public string Ref { get; set; }

        public static string GetStatusProc(string response)
        {
            JObject obj = JObject.Parse(response);
            IEnumerable<ResponseOperation> list  = JsonConvert.DeserializeObject<IEnumerable<ResponseOperation>>(obj["ops"].ToString());
            string statusProc = string.Empty;
            foreach (var item in list)
            {
                statusProc = item.Proc;
            }
            return statusProc;
        }
    }
}
