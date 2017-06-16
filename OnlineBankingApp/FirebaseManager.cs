using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp
{
    public class FirebaseManager
    {
        private string host = "https://onlinebankdb.firebaseio.com/privatdb";
        private string table;
        private string merchId;
        private string format = ".json";
        private string auth = "?auth=";
        private string secretKey = "your api key here";

        public FirebaseManager() { }

        public FirebaseManager(string _table, [Optional]string _merchId)
        {
            this.table = _table;
            this.merchId = _merchId;
        }

        public async Task<string> GetContentAsync([Optional]string section)
        {
            string requestUriString;
            if (String.IsNullOrEmpty(section))
            {
                requestUriString = host + "/" + table + "/" + merchId + format + auth + secretKey;
            }
            else
            {
                requestUriString = host + "/" + table + "/" + merchId + "/" + section + format + auth + secretKey;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Method = "GET";

            StringBuilder output = new StringBuilder();
            try
            {
                WebResponse response = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                output.Append(sr.ReadToEnd());
            }
            catch (WebException exp)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + exp.Message);
            }
            return output.ToString();
        }
    }
}
