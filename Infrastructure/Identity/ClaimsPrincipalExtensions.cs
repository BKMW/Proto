using Core.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)

        {

            if (!principal.Identity.IsAuthenticated)

                return null;


            //ClaimsPrincipal currentUser = principal;

            return principal.FindFirst(ClaimTypes.NameIdentifier).Value;

        }

        public static string GetUserName(this ClaimsPrincipal principal)

        {

            if (!principal.Identity.IsAuthenticated)

                return null;

           // ClaimsPrincipal currentUser = principal;

            return principal.FindFirst(ClaimTypes.Name).Value;

        }

        public static string GetRole(this ClaimsPrincipal principal)

        {

            if (!principal.Identity.IsAuthenticated)

                return null;

           // ClaimsPrincipal currentUser = principal;

            return principal.FindFirst(ClaimTypes.Role).Value;

        }

       
        public static string GetJti(this ClaimsPrincipal principal)

        {

            if (!principal.Identity.IsAuthenticated)

                return null;

           // ClaimsPrincipal currentUser = principal;

            return principal.FindFirst(JwtRegisteredClaimNames.Jti).Value;

        }

        public static List<Claim> GetPermissions(this ClaimsPrincipal principal)

        {

            if (!principal.Identity.IsAuthenticated)

                return null;

          
            return principal.FindAll("Permission")?.ToList();

        }
        public static ClaimsCurrentUser GetCurrentUser(this ClaimsPrincipal principal)

        {

            if (!principal.Identity.IsAuthenticated)

                return null;

            // ClaimsPrincipal currentUser = principal;

            return new ClaimsCurrentUser {
                Role= principal.FindFirst(ClaimTypes.Role).Value ,
                UserId = principal.FindFirst(ClaimTypes.NameIdentifier).Value,
                UserName = principal.FindFirst(ClaimTypes.Name).Value,
                Jti = principal.FindFirst(JwtRegisteredClaimNames.Jti).Value,

            };

        }
    }
}
