using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using OnlineBankingApp.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace OnlineBankingApp
{
    class User
    {
        private static string merchant;
		//This fields for testing work of project
        private string myCard1 = "your card_number here";
        private string myCard2 = "- || -";
        private string myCard3 = "- || -";
        private static List<string> cards;
		
        public User(string _merchId)
        {
            merchant = _merchId;
            cards = new List<string>();
            cards.Add(myCard1);
            cards.Add(myCard2);
            cards.Add(myCard3);
        }

        public static List<string> Cards
        {
            get { return cards; }
        }
        public static string MerchId { get { return merchant; } }

        public static async Task<string> GetCountry()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://freegeoip.net/xml/");
            request.Method = "GET";
            request.ContentType = "application/xml";
            request.Accept = "application/xml";

            StringBuilder locationResponse = null;
            try
            {
                WebResponse response = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                locationResponse = new StringBuilder();
                locationResponse.Append(sr.ReadToEnd());
            }
            catch(WebException exp)
            {
                throw new Exception("Error: " + exp.Message);
            }
            var responseXml = XDocument.Parse(locationResponse.ToString()).Element("Response");
            return responseXml.Element("CountryName").Value;
        }

    }
}
