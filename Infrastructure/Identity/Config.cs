using Application.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Identity
{
    public  class Config
    {
        private static readonly byte[] key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);

        public static readonly TokenValidationParameters tokenValidationParams = new TokenValidationParameters
        {
            //ValidateIssuerSigningKey = true,
            //IssuerSigningKey = new SymmetricSecurityKey(key),
            //ValidateIssuer = false,
            //ValidateAudience = false,
            //ValidateLifetime = true,
            //RequireExpirationTime = false,
            //ClockSkew = TimeSpan.Zero

            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            // ValidIssuer = AuthorizationConstants.Site,
            // ValidAudience = AuthorizationConstants.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        public static readonly PasswordOptions passwordOptions = new PasswordOptions()
        {
            RequiredLength = 6,
            RequiredUniqueChars = 2,
            RequireDigit = false,
            RequireLowercase = false,
            RequireNonAlphanumeric = false,
            RequireUppercase = false
        };
        #region GenerateRandomPassword


        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = passwordOptions;

            string[] randomChars = new[] {
                    "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                    "abcdefghijkmnopqrstuvwxyz",    // lowercase
                    "0123456789",                   // digits
                    "!@$?_-"                        // non-alphanumeric
            };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
        #endregion


    }
}
