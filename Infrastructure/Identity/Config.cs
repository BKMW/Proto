

using Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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

       


    }
}
