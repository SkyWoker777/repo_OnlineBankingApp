using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.DataOfProcess
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MobileRefill
    {
        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Beeline Украина - RPBLUA | 
        /// Киевстар - RPKSTR |
        /// life :) - RPLIFE |
        /// МТС Украина - RPMTSU |
        /// PEOPLEnet - PPNET |
        /// Utel - RPUTEL |
        /// Интернет Киевстар - RPKSIN
        /// </summary>
        [JsonProperty(PropertyName = "operator")]
        public string Operator { get; set; }

        [JsonProperty(PropertyName = "payment_id")]
        public string PaymentId { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
    }
}
