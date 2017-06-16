using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBankingApp.DataOfProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResponseStatus
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string Status { get; set; }

        /// <summary>
        ///  Request a status of the payment made.
        /// </summary>
        /// <param name="paymentId">уникальный идентификатор платежа, присвоенный мерчантом</param>
        /// <param name="refPB">внутренний референс платежа в Приват24</param>
        /// <param name="merchId">merchant</param>
        /// <param name="convId">Process Id</param>
        public async void RequestPaymentStatus(string paymentId, string refPB, string merchId, string convId)
        {
            PaymentStatus status = new PaymentStatus()
            {
                Id = paymentId,
                Ref = refPB,
            };

            JObject data = JObject.FromObject(status);
            data.Add("merchant", merchId);
            Operation operation = new Operation()
            {
                Ref = Util.GetRef(),
                Type = "create",
                Obj = "task",
                Conv_Id = convId,
                Data = data,
            };

            CorezoidMessage message = new CorezoidMessage();
            message.Create(operation, Util.ApiLogin, Util.ApiSecret);
            await message.SendAsync();
        }
    }
}
