using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Operation
    {
        [JsonProperty(PropertyName = "ref", Required = Required.Default)]
        public string Ref { get; set; }
        /// <summary>
        /// create or modify
        /// </summary>
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string Type { get; set; }
        /// <summary>
        /// task
        /// </summary>
        [JsonProperty(PropertyName = "obj", Required = Required.Always)]
        public string Obj { get; set; }
        /// <summary>
        /// Process Id
        /// </summary>
        [JsonProperty(PropertyName = "conv_id", Required = Required.Always)]
        public string Conv_Id { get; set; }
        /// <summary>
        /// Data of current process for the task to be created
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public JObject Data { get; set; }
    }
}
