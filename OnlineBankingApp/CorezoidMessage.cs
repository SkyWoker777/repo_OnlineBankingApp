using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBankingApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace OnlineBankingApp
{
    public class CorezoidMessage
    {
        private string host = "https://www.corezoid.com/api/1/json";
        private string API_LOGIN;
        private string API_SECRET;
        private string signature;
        private string unixTime;
        private static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public string Body { get; private set; }

        protected static long GetCurrentUnixTimestampSeconds()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }

        public CorezoidMessage() { }

        public void Create(Operation operaion, string apiLogin, string apiSecret)
        {
            JObject obj = new JObject();
            try
            {
                JObject ops = JObject.FromObject(operaion);
                obj.Add("ops", new JArray(ops));
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine("Json serialize error: {0}", ex.Message);
            }

            API_LOGIN = apiLogin;
            API_SECRET = apiSecret;
            Body = obj.ToString();
            unixTime = GetCurrentUnixTimestampSeconds().ToString();
            signature = GenerateSignature(unixTime, Body);
        }

        public void Create(List<Operation> ops, string apiLogin, string apiSecret)
        {
            JObject obj = new JObject();
            try
            {
                obj["ops"] = JToken.FromObject(ops);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Json serialize error: {0}", ex.Message);
            }

            API_LOGIN = apiLogin;
            API_SECRET = apiSecret;
            Body = obj.ToString();
            unixTime = GetCurrentUnixTimestampSeconds().ToString();
            signature = GenerateSignature(unixTime, Body);
        }

        /// <summary>
        /// Calculate Signature for Corezoid: {SIGNATURE} = hex( sha1({GMT_UNIXTIME} + {API_SECRET} + {CONTENT} + {API_SECRET})) 
        /// </summary>
        /// <param name="unixTime">GMT_UNIXTIME</param>
        /// <param name="content">{ ops:[ { } ] }</param>
        /// <returns>string signature</returns>
        private string GenerateSignature(string unixTime, string content)
        {
            string text = unixTime + API_SECRET + content + API_SECRET;

            Windows.Storage.Streams.IBuffer data = CryptographicBuffer.ConvertStringToBinary(text, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider sha1 = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            Windows.Storage.Streams.IBuffer buffer = sha1.HashData(data);
            string strHex= CryptographicBuffer.EncodeToHexString(buffer).ToLower();
            return strHex;
        }

        public async Task<string> SendAsync()
        {
            string requestUriString = host + "/" + API_LOGIN + "/" + unixTime + "/" + signature;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Method = "POST";

            Stream stream = await request.GetRequestStreamAsync();
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(Body);
            }
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
            System.Diagnostics.Debug.WriteLine("OUTPUT Message is:" + output.ToString());

            return output.ToString();
        }
    }
}
