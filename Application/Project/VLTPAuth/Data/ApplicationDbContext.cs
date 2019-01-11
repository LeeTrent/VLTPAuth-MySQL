using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VLTPAuth.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
   
            /////////////////////////////////
            // Define database schema
            /////////////////////////////////
            builder.HasDefaultSchema("VLTP");

            //////////////////////////////////////////////////////////////////////////////////////////
            // Application User (AspNetUsers table):
            //////////////////////////////////////////////////////////////////////////////////////////
            //  - This is being done to fix the following problem:
            //  - "No coercion operator is defined between types 'System.Int16' and 'System.Boolean'"
            //  - This problem is encountered when using a MySQL database
            //////////////////////////////////////////////////////////////////////////////////////////
            builder.Entity<ApplicationUser>(au =>
            {
              au.Property(u => u.EmailConfirmed).HasColumnType("tinyint(1)");
              au.Property(u => u.PhoneNumberConfirmed).HasColumnType("tinyint(1)");
              au.Property(u => u.TwoFactorEnabled).HasColumnType("tinyint(1)");
              au.Property(u => u.LockoutEnabled).HasColumnType("tinyint(1)");
});



        }
    }
}
