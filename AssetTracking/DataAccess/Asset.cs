using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AssetTracking.DataAccess
{
    public class Asset
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PriceInUsd { get; set; }
        public int OfficeId { get; set; }
        public AssetType AssetType { get; set; }
        public int AssetTypeId { get; set; }
    }

    public class AssetConfig : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(a => a.AssetType).WithMany().HasForeignKey(a => a.AssetTypeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasData(new Asset { Id = 1,  AssetTypeId =1, OfficeId =1, Model = "Lenovo sge3", PriceInUsd =799, PurchaseDate = new DateTime(2015-12-03) });
            builder.HasData(new Asset { Id = 2, AssetTypeId = 2, OfficeId = 1,  Model = "Ihpone 10", PriceInUsd = 699, PurchaseDate = new DateTime(2019 - 12 - 03) });
            builder.HasData(new Asset { Id = 3, AssetTypeId = 1, OfficeId = 2, Model = "Apple MacBook", PriceInUsd = 1499, PurchaseDate = new DateTime(2016 - 03 - 03) });
            builder.HasData(new Asset { Id = 4, AssetTypeId = 2, OfficeId = 2, Model = "Nokia 5110", PriceInUsd = 1499, PurchaseDate = new DateTime(2012 - 12 - 03) });



        }
    }
}
