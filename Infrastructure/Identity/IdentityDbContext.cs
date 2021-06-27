using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Identity
{
    public class IdentityDbContext : IdentityDbContext<User>
   
    {
        #region properties

        //[Obsolete]
        //public virtual DbSet<UserListResponseDto> SuiviUsers { get; set; }
        //[Obsolete]
        //public virtual DbSet<UserListInfoDto> InfoUsers { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        #endregion

        #region ctor

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        #endregion ctor
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //builder.Ignore<IdentityUserToken<string>>();
            //builder.Ignore<IdentityRoleClaim<string>>();
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserLogin<string>>();


            //builder.Ignore<UserListResponseDto>();
            //builder.Ignore<UserListInfoDto>();



            builder.Seed();
        }  


    }
}
