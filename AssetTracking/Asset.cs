using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace AssetTracking
{

    public class Asset
    {
        public int Id { get; private set; }
        public int TypeId { get; private set; }
        public string ModelName { get; private set; }
        private readonly int LifeTimeInYears = 3;
        public DateTime PurchaseDate { get; set; }
        public decimal PriceInUsd { get; private set; }

        private bool LifeTimeLeft(int months)
        {
            int daysLeftToReachLifeTime = (int)(this.PurchaseDate.AddYears(this.LifeTimeInYears) - DateTime.Today).TotalDays;
            return daysLeftToReachLifeTime < months * 30 && daysLeftToReachLifeTime > 0;
        }

        public void PrintToConsole(string localCurrencyCode, decimal conversionRateToLocalCurrency)
        {
            Console.BackgroundColor = this.LifeTimeLeft6Months ? (this.LifeTimeLeft3Months ? ConsoleColor.Red : ConsoleColor.Yellow) : ConsoleColor.Black;
            Console.WriteLine(ToString(localCurrencyCode, conversionRateToLocalCurrency));
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void PrintToConsoleWithId(string localCurrencyCode, decimal conversionRateToLocalCurrency)
        {
            Console.BackgroundColor = this.LifeTimeLeft6Months ? (this.LifeTimeLeft3Months ? ConsoleColor.Red : ConsoleColor.Yellow) : ConsoleColor.Black;
            Console.WriteLine(ToString(localCurrencyCode, conversionRateToLocalCurrency));
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private bool LifeTimeLeft3Months => LifeTimeLeft(3);
        private bool LifeTimeLeft6Months => LifeTimeLeft(6);

        public string ToString(string localCurrencyCode, decimal conversionRateToLocalCurrency)
        {
            return string.Format("{0} {1} Price : {2} {3}",this.Id , this.ModelName, Convert.ToInt32(conversionRateToLocalCurrency * this.PriceInUsd), localCurrencyCode);
        }

        public Asset(int typeId, string modelName, DateTime purchaseDate, decimal priceInUsd)
        {
            this.TypeId = typeId;
            this.ModelName = modelName;
            this.PurchaseDate = purchaseDate;
            this.PriceInUsd = priceInUsd;
        }

        public Asset(int id,  int typeId, string modelName, DateTime purchaseDate, decimal priceInUsd) : this(typeId, modelName, purchaseDate, priceInUsd)
        {
            this.Id = id;
        }
    }


}