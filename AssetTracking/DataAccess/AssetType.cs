using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AssetTracking.DataAccess
{
    public class AssetType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AssetTypeConfig : IEntityTypeConfiguration<AssetType>
    {
        public void Configure(EntityTypeBuilder<AssetType> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasData(new AssetType { Id = 1, Name = "Laptop computer" });
            builder.HasData(new AssetType { Id = 2, Name = "Mobile phone" });
        }
    }
}
