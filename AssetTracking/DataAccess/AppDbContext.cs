using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using AssetTracking.DataAccess;
using Microsoft.IdentityModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AssetTracking.DataAccess
{
    public class AppDbContext :  IdentityDbContext<ApplicationUser> //DbContext
    {
        public const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AssetTracking;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public DbSet<DataAccess.Office> Offices { get; set; }
        public DbSet<DataAccess.Asset> Assets { get; set; }
        public DbSet<DataAccess.AssetType> AssetTypes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public AppDbContext() 
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new  OfficeConfig());
            modelBuilder.ApplyConfiguration(new  AssetTypeConfig());
            modelBuilder.ApplyConfiguration(new  AssetConfig());

        }
    }

}






