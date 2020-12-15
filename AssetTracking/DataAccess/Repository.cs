using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using AssetTracking.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace AssetTracking.DataAccess

{
    public class Repository
    {
        private  AppDbContext DBContext;
        //private IUserCreationService UserManager;
        private UserManager<ApplicationUser> UserManager;

        //private Auth<ApplicationUser> LoginManager;
        public Repository(/*UserManager<ApplicationUser> userManager*/)
        {
            DBContext = new AppDbContext();
            UserManager = Program.Services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
    
           
     
        }
    public IEnumerable<AssetTracking.Office> AllCompanyAssets()
        {
            return DBContext.Offices.Include(o => o.Assets).Select(o => new AssetTracking.Office( o.Location, o.LocalCurrencyCode,
                o.Assets.Select(a => new AssetTracking.Asset(a.Id, a.AssetTypeId, a.Model,a.PurchaseDate, a.PriceInUsd))
                ));
        }

        public bool CreateUser(string name, string password)
        {
            var user = new ApplicationUser { UserName = name };
            var a = UserManager.CreateAsync(user, password).GetAwaiter();
            return a.GetResult().Succeeded;
        }

        public bool CheckLoginCredentials(string name, string password)
        {
            var user = DBContext.Users.Where(u => u.UserName == name).FirstOrDefault();
            //var applicationUser = new ApplicationUser { UserName = name };
            var a =this.UserManager.CheckPasswordAsync(user,password).GetAwaiter();
         
            return a.GetResult();

        }

        public IEnumerable<AssetTracking.Office> AllOffices()
        {
            return DBContext.Offices.Select(o => new AssetTracking.Office(o.Id, o.Location));
        }

        public IEnumerable<AssetTracking.AssetType> AllAssetTypes()
        {
            return DBContext.AssetTypes.Select(aT => new AssetTracking.AssetType(aT.Id, aT.Name));
        }

        public void AddAsset(int officeId, AssetTracking.Asset newAsset)
        {
            var existingOffice  = DBContext.Offices.Include(o => o.Assets).Where(o => o.Id == officeId).FirstOrDefault();
            existingOffice.Assets.Add(new DataAccess.Asset { Model = newAsset.ModelName,  PriceInUsd = newAsset.PriceInUsd , PurchaseDate = newAsset.PurchaseDate, AssetTypeId = newAsset.TypeId });
            DBContext.SaveChanges();
        }

        public void AddOffice(AssetTracking.Office office)
        {
             DBContext.Offices.Add(new DataAccess.Office { Location = office.Location, LocalCurrencyCode = office.LocalCurrencyCode });
             DBContext.SaveChanges();
        }


        public void AddAssetType(AssetTracking.AssetType assetType)
        {
            DBContext.AssetTypes.Add(new DataAccess.AssetType { Name = assetType.Name});
            DBContext.SaveChanges();
        }

        public void DeleteAssetType(int id)
        {
            var assetType = DBContext.AssetTypes.Where(a => a.Id == id).FirstOrDefault();
            if (assetType != null)
            {
                DBContext.AssetTypes.Remove(assetType);
                DBContext.SaveChanges();
            }
        }

        public void DeleteOffice(int id)
        {
            var office = DBContext.Offices.Where(a => a.Id == id).FirstOrDefault();
            if (office != null)
            {
                DBContext.Offices.Remove(office);
                DBContext.SaveChanges();
            }
        }



        public void DeleteAsset(int id)
        {
            var asset = DBContext.Assets.Where(a => a.Id == id).FirstOrDefault();
            if (asset != null)
            {
                DBContext.Assets.Remove(asset);
                DBContext.SaveChanges();
            }
        }
    }
    }

