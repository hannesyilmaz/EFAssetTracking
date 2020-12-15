using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using AssetTracking.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AssetTracking
{
    partial class Program : IDesignTimeDbContextFactory<DataAccess.AppDbContext>
    {
        private static DataAccess.Repository Repository;
        public static ServiceCollection Services;
        static void Main(string[] args)
        {
            
            Services = new ServiceCollection();
            Services.AddDbContext<AppDbContext>(
           options =>
           {
               options.UseSqlServer(AppDbContext.ConnectionString);
           });

            // Authentification
            Services.AddIdentityCore<ApplicationUser>(options =>
            {
                // Configure identity options
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AppDbContext>();
            Repository = new DataAccess.Repository();

            PageLogin();

        }

        public static void PageLogin()
        {
            Header("============== AsssetTracker Website =============  \n Login or Sign in");

            Console.WriteLine("Enter s for a new user sign in or l for an existing user login: ");
            var signOrLogInput = Console.ReadLine();
            if (signOrLogInput == "s")
            {
                Console.Clear();
                PageRegisterNewUser();
            }
            else if (signOrLogInput == "l")
            {
                AssetLogin();
            }
            
        }

        public static void AssetLogin()
        {
            Header("Login");
            Console.Write("Enter user name: ");
            var nameInput = Console.ReadLine();
            Console.Write("Enter password: ");
            var passwordInput = Console.ReadLine();

            bool isSuccesful = Repository.CheckLoginCredentials(nameInput, passwordInput);

            if (isSuccesful)
            {
                PageMainMenu();
            }
            else
            {
                Console.Write("Username or password was incorrect");
                Console.ReadLine();
                Console.Clear();
                PageLogin();
            }



        }

        public static void PageRegisterNewUser()
        {
            Header("Register new user");
            Console.WriteLine("Enter c if you want to cancel registration");
            Console.WriteLine("Enter user name");
            var nameInput = Console.ReadLine();
            if (nameInput == "c")
            {
                Console.Clear();
                PageLogin();
            }
            Console.WriteLine("Enter password");
            var passwordInput = Console.ReadLine();
            if (passwordInput == "c")
            {
                Console.Clear();
                PageLogin();
            }
            bool isSuccesful = Repository.CreateUser(nameInput, passwordInput);
            if (!isSuccesful)
            {
                Console.WriteLine("Registration failed");
                Console.ReadLine();
                Console.Clear();
                PageRegisterNewUser();
            }
            Console.WriteLine("Registration was created succesfully");
            Console.ReadLine();
            PageLogin();
        }

       
        private static void PageMainMenu()
        {
            Header("Main menu");


            Console.WriteLine("What do you want to do?");
            Console.WriteLine("a) Read all company assets");
            Console.WriteLine("b) Add a new office");
            Console.WriteLine("c) Delete an office");
            Console.WriteLine("d) Add a new asset");
            Console.WriteLine("e) Delete an asset");
            Console.WriteLine("f) Add a new asset type");
            Console.WriteLine("g) Delete asset type");
            Console.WriteLine("h) Logout");


            ConsoleKey command = Console.ReadKey(true).Key;

            if (command == ConsoleKey.A)
                PageReadAllCompanyAssets();

            if (command == ConsoleKey.B)
                PageAddNewOffice();

            if (command == ConsoleKey.C)
                PageDeleteOffice();

            if (command == ConsoleKey.D)
                PageAddAsset();

            if (command == ConsoleKey.E)
                PageDeleteAsset();

            if (command == ConsoleKey.F)
                PageAddNewAssetType();

            if (command == ConsoleKey.G)
                PageDeleteAssetType();

            if (command == ConsoleKey.H)
                Logout();
            
            if (command != ConsoleKey.H)
            {
                Console.ReadLine();
                PageMainMenu();
            }

        }


        private static void Header(string text)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine(text.ToUpper());
            Console.WriteLine();
        }

        private static void Logout()
        {
            Console.Clear();
            PageLogin();
        }

        public static void PageReadAllCompanyAssets()
        {
            Header("All company assets");
            Repository = new DataAccess.Repository();
            foreach (Office a in Repository.AllCompanyAssets())
                a.WriteAsset();
            Console.WriteLine("Press Enter key to return to main menu");


        }

        public static void PageDeleteOffice()
        {
            Header("Delete office");
          
            Console.WriteLine(string.Format("Enter the id of the office you want to delete"));

            foreach (Office o in Repository.AllOffices())
                Console.WriteLine(string.Format("{0}) {1}",o.Id, o.Location));
            var enteredId = Convert.ToInt32(Console.ReadLine());
           
            Repository.DeleteOffice(enteredId);
            Console.WriteLine("The office was deleted succesfully.");
            Console.WriteLine("Press any key to return to main menu");

        }

        public static void PageAddNewOffice()
        {

            Header("Add new office");
            Console.WriteLine("Enter the location");
            var locationInput = Console.ReadLine();
            Console.WriteLine("Enter the local currency code");
            var localCurrencyCodeInput = Console.ReadLine();
            var newOffice = new Office(locationInput, localCurrencyCodeInput);
            Repository.AddOffice(newOffice);
            Console.WriteLine("The office was added succesfully");
            Console.WriteLine("Press any key to return to main menu");
        }



        public static void PageAddNewAssetType()
        {
            Header("Add new asset type");
            Console.WriteLine("Enter the name");
            var nameInput = Console.ReadLine();
            var newAssetType = new AssetType(nameInput);
            Repository.AddAssetType(newAssetType);
            Console.WriteLine("The asset type was added succesfully.");
            Console.WriteLine("Press any key to return to main menu");
        }
        public static void PageAddAsset()
        {
            Header("Add new asset");
            Console.WriteLine(string.Format("Enter the id of the office you want to add an asset to"));
            foreach (var o in Repository.AllOffices())
                Console.WriteLine(string.Format("{0}) {1}", o.Id, o.Location));
            var officeIdInput = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(string.Format("Enter the id of the asset type"));
            foreach (var o in Repository.AllAssetTypes())
                Console.WriteLine(string.Format("{0}) {1}", o.Id, o.Name));
            var assetTypeIdInput = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the model name");
            var modelNameInput = Console.ReadLine();
            Console.WriteLine("Enter price in dollars");
            var priceInDollarsInput = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Enter purchase date in format YYYY-MM-DD or leave it empty for todays date");
            var dateInstring = Console.ReadLine();
            var purchaseDateInput = dateInstring == string.Empty ? DateTime.Today : Convert.ToDateTime(dateInstring);

            var asset= new AssetTracking.Asset(assetTypeIdInput, modelNameInput, purchaseDateInput, priceInDollarsInput);
            Repository.AddAsset(officeIdInput, asset);
            Console.WriteLine("The asset was added succesfully");
            Console.WriteLine("Press any key to return to main menu");
        }


        public static void PageDeleteAsset()
        {
            Header("Delete asset");
            foreach (Office a in Repository.AllCompanyAssets())
                a.WriteAsset();
            Console.WriteLine(string.Format("Enter the id of the asset you want to delete"));
            var idInput = Console.ReadLine();
            Repository.DeleteAsset(Convert.ToInt32(idInput));
            Console.WriteLine("The asset was deleted succesfully");
            Console.WriteLine("Press any key to return to main menu");
        }

        public static void PageDeleteAssetType()
        {
            Header("Delete asset type");

            foreach (var o in Repository.AllAssetTypes())
                Console.WriteLine(string.Format("{0}) {1}", o.Id, o.Name));
            Console.WriteLine(string.Format("Enter the id of the asset type you want to delete"));
            var idInput = Convert.ToInt32(Console.ReadLine());
            Repository.DeleteAssetType(idInput);
            Console.WriteLine("The asset type was deleted succesfully");
            Console.WriteLine("Press any key to return to main menu");

        }


        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(AppDbContext.ConnectionString);
            return new AppDbContext(builder.Options);
        }
    }

    public interface IUserCreationService
    {
        Task CreateUser();
    }


  
}

