using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AssetTracking.DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? DeactivationDate { get; set; }
        public ApplicationUser() : base()
        { }
        public ApplicationUser(string username) : base(username)
        { }
    }

    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
        }
    }
}
