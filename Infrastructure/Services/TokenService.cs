﻿using Application.Constants;
using Application.Dtos;
using Application.Interfaces;
using Application.Tools;
using Infrastructure.Identity;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        #region ctor
        private readonly AppSettings _appSettings;
        private readonly IdentityDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;




        public TokenService(
                IOptions<AppSettings> appSettings,
                IdentityDbContext context,
                UserManager<User> userManager,
                RoleManager<IdentityRole> roleManager

            )
        {
            _appSettings = appSettings?.Value;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        #endregion

        #region GenerateTokenAsync
        public async Task<ApiResponse> GenerateTokenAsync(ClaimsCurrentUser claimsCurrentUser)
        {

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY));
            var subject = new ClaimsIdentity(new[]
                {
                        new Claim(ClaimTypes.Name, claimsCurrentUser.UserName),
                        new Claim(ClaimTypes.NameIdentifier, claimsCurrentUser.UserId),
                        new Claim(ClaimTypes.Role, claimsCurrentUser.Role),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     //   new Claim("LoggedOn", DateTime.Now.ToString()),
                });

            var role = await _roleManager.FindByNameAsync(claimsCurrentUser.Role);

            var roleClaims = await _roleManager.GetClaimsAsync(role);
              
             subject.AddClaims(roleClaims);
         

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.ExpireTime), // 5-10 
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,//JwtRegisteredClaimNames.Jti
                UserId = claimsCurrentUser.UserId,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = tokenDescriptor.Expires,
                Token = Operations.GenerateRefreshToken()
        };

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == claimsCurrentUser.UserId && x.ExpiryDate > DateTime.UtcNow);


            await _context.RefreshTokens.AddAsync(refreshToken);
            var res = await _context.SaveChangesAsync();

            if (res < 0)
                return new ApiResponse { resultCode = Result.ERROR };

            if (storedToken != null)
                return new ApiResponse { resultCode = Result.MULTI_AUTH, token = jwtToken, refreshToken = refreshToken.Token };

            return new ApiResponse { resultCode = Result.SUCCESS, token = jwtToken, refreshToken = refreshToken.Token };

        }
        #endregion

        #region RefreshTokenAsync
        public async Task<ApiResponse> RefreshTokenAsync(string refreshToken, ClaimsCurrentUser claimsCurrentUser)
        {

            try
            {
                #region verification

                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

                if (storedToken == null)
                    return new ApiResponse { resultCode = Result.TOKEN_NOTFOUND };



                if (storedToken.JwtId != claimsCurrentUser.Jti)
                    return new ApiResponse { resultCode = Result.TOKEN_NOTMATCH };
                #endregion

                #region refreshToken
                // Generate a new token

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY));
                var subject = new ClaimsIdentity(new[]
               {
                        new Claim(ClaimTypes.Name,claimsCurrentUser.UserName),
                        new Claim(ClaimTypes.NameIdentifier, claimsCurrentUser.UserId),
                        new Claim(ClaimTypes.Role, claimsCurrentUser.Role),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     //   new Claim("LoggedOn", DateTime.Now.ToString()),
                });

                var role = await _roleManager.FindByNameAsync(claimsCurrentUser.Role);

                var roleClaims = await _roleManager.GetClaimsAsync(role);

                subject.AddClaims(roleClaims);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = subject,
                    Expires = DateTime.UtcNow.AddSeconds(_appSettings.ExpireTime), // 5-10 
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                var jwtToken = tokenHandler.WriteToken(token);

                // update current token 

                storedToken.JwtId = token.Id;
                storedToken.AddedDate = DateTime.UtcNow;
                storedToken.ExpiryDate = tokenDescriptor.Expires;
                storedToken.Token = Operations.GenerateRefreshToken();


                _context.RefreshTokens.Update(storedToken);
                var res = await _context.SaveChangesAsync();
                if (res < 0)
                    return new ApiResponse { resultCode = Result.ERROR };

                return new ApiResponse { resultCode = Result.SUCCESS, token = jwtToken, refreshToken = storedToken.Token };
                #endregion


            }
            catch (Exception ex)
            {

                return new ApiResponse { resultCode = Result.EXCEPTION };

            }
        }


        #endregion

      
    }
}