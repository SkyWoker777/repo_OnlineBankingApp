using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp
{
    static class Util
    {
        private static readonly string API_LOGIN = "login webhook here";
        private static readonly string API_SECRET = "write api_key here";
        private static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string ApiLogin
        {
            get { return API_LOGIN; }
        }

        public static string ApiSecret
        {
            get { return API_SECRET; }
        }

        public static string GetRef()
        {
            return ((long)(DateTime.UtcNow - UnixEpoch).TotalMilliseconds + new Random(DateTime.UtcNow.Millisecond).Next(1000)).ToString();
        }
        public static string GetPaymentId()
        {
            return (new Random(DateTime.UtcNow.Millisecond).Next(10000000)).ToString();
        }
    }
}
