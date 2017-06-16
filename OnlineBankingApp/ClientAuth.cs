using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace OnlineBankingApp
{
    class ClientAuth
    {
        private static string URL = "https://link.privatbank.ua/api/auth/";
        private static string CLIENT_ID = "your api_id here";
        private static string CLIENT_SECRET = "your api secret here";
        Session session;

        public ClientAuth()
        {
            session = new Session();
        }
        public async void GetSessionId()
        {
            JObject data = new JObject();
            data.Add("clientId", CLIENT_ID);
            data.Add("clientSecret", CLIENT_SECRET);
            string result = await Post("createSession", data.ToString());
            System.Diagnostics.Debug.WriteLine("Create: " + result);
            try
            {
                JObject obj = JObject.Parse(result);
                session = JsonConvert.DeserializeObject<Session>(obj.ToString());
                System.Diagnostics.Debug.WriteLine("ID is :" + session.ID);
            }
            catch(JsonException ex)
            {
                throw new JsonException(String.Format("Json error. %s", ex.Message));
            }

            //validate session
            data = new JObject();
            data.Add("sessionId", session.ID);
            result = await Post("validateSession", data.ToString());
            System.Diagnostics.Debug.WriteLine("Validate session: " + result);

        }

        private async void RemoveSession()
        {
            JObject data = new JObject();
            data.Add("sessionId", session.ID);
            string result = await Post("removeSession", data.ToString());
            System.Diagnostics.Debug.WriteLine("Success: " + result);
        }

        public async Task<string> Post(string uri, string data)
        {
            string requestUriString = URL + uri;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Method = "POST";

            Stream stream = await request.GetRequestStreamAsync();
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(data);
            }
            StringBuilder output = null;
            try
            {
                WebResponse response = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request); 
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                output = new StringBuilder();
                output.Append(sr.ReadToEnd());
            }
            catch(WebException exp)
            {
                System.Diagnostics.Debug.WriteLine(exp.Message);
            }
            return output.ToString();
        }
    }
}
