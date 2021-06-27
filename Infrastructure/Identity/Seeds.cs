using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Identity
{
    public static class IdentitySeeds
    {
        public static void Seed(this ModelBuilder builder)
        {
            #region roles
            IdentityRole adminRole = new IdentityRole { Id = "1", Name = "ADMIN", NormalizedName = "ADMIN" };
            IdentityRole adminBankRole = new IdentityRole { Id = "2", Name = "ADMIN COMPANY", NormalizedName = "ADMIN COMPANY" };

            builder.Entity<IdentityRole>().HasData(
                adminRole,
                adminBankRole

               );
            #endregion

            #region users
            User admin = new User
            {

                Id = "92003b37-4eb0-468c-8acd-47a2b70c0654  ",//Guid.NewGuid().ToString() ,
                UserName = "ADMIN",
                NormalizedUserName = "ADMIN",
                PasswordHash = "AQAAAAEAACcQAAAAEHJUf19zuMUwI7BK5T1ris52fQMjSY8WmTkVdYpjq94vQqnxfGbxfkzANkzgAGTOfQ==",
                SecurityStamp = "IIKPNARWI7BSHAFAH34MKIXPNYESITBZ",
                ConcurrencyStamp = "b27a1ca3-6049-4f9a-b84f-1d2f9fdcb471",
                LockoutEnabled=true,
                Addeddate= DateTime.Now
            };

            IdentityUserRole<string> userRole = new IdentityUserRole<string> { UserId = admin.Id, RoleId = "1" };

            builder.Entity<User>().HasData(
                 admin 

              );

            builder.Entity<IdentityUserRole<string>>().HasData(
                userRole

             );

            #endregion

        }
    }
}
