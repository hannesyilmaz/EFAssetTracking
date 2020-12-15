using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AssetTracking
{
    public class Office
    {
        public int Id { get; private set; }
     
        static HttpClient client = new HttpClient();
        public string Location { get; private set; }
        public readonly string LocalCurrencyCode;
        private readonly string UrlForCurrencyRates = "https://api.exchangeratesapi.io/latest?base=USD";
        public IEnumerable<Asset> Assets { get; private set; }

        public void WriteAsset()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine(this.Location);
            Console.BackgroundColor = ConsoleColor.Black;
            foreach (var a in this.Assets)
                a.PrintToConsole(this.LocalCurrencyCode, GetConversionRateForLocalCurrency().Result);
        }



        public async Task<decimal> GetConversionRateForLocalCurrency()
        {
            HttpResponseMessage response = await client.GetAsync(UrlForCurrencyRates);
            var content = response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(content.Result);
            var rates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(data["rates"].ToString());
            var rate = rates[LocalCurrencyCode.ToUpper()];
            return rate;
        }

        public Office(string location, string localCurrencyCode)
        {
            this.Location = location;
            this.LocalCurrencyCode = localCurrencyCode;
        }

        public Office(int id, string location)
        {
            this.Id = id;
            this.Location = location;
        }

        public Office(string location, string localCurrencyCode, IEnumerable<AssetTracking.Asset> assets) :this(location, localCurrencyCode)
        {
            this.Location = location;
            this.LocalCurrencyCode = localCurrencyCode;
            this.Assets = assets;
        }
        
    }
}