using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AssetTracking.DataAccess
{
    public class Office
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string LocalCurrencyCode { get; set; } 
        public ICollection<Asset> Assets { get; set; }
    }

    public class OfficeConfig : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasMany(o => o.Assets).WithOne().HasForeignKey(a => a.OfficeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasData(new Office { Id =1, Location = "London",  LocalCurrencyCode= "GBP" });
            builder.HasData(new Office { Id = 2, Location = "New York", LocalCurrencyCode = "USD" });
        }
    }
}
