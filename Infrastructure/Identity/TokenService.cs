using Core.Constants;
using Core.Dtos;
using Core.Interfaces;
using Core.Models.Identity;
using Core.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class TokenService : ITokenService
    {
        #region ctor
        private readonly AppSettings _appSettings;
        private readonly IdentityDbContext _context;
        private readonly UserManager<User> _userManager;

        private readonly TokenValidationParameters _tokenValidationParams;



        public TokenService(
                IOptions<AppSettings> appSettings,
                IdentityDbContext context,
                UserManager<User> userManager,
                TokenValidationParameters tokenValidationParams

            )
        {
            _appSettings = appSettings?.Value;
            _context = context;
            _userManager = userManager;
            _tokenValidationParams = tokenValidationParams;

        }
        #endregion

        #region GenerateTokenAsync
        public async Task<ApiResponse>GenerateTokenAsync(ClaimsCurrentUser claimsCurrentUser)
        {

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY));
           
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
               {
                        new Claim(ClaimTypes.Name, claimsCurrentUser.UserName),
                        new Claim(ClaimTypes.NameIdentifier, claimsCurrentUser.UserId),
                        new Claim(ClaimTypes.Role, claimsCurrentUser.Role),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     //   new Claim("LoggedOn", DateTime.Now.ToString()),
                }),
                Expires = DateTime.UtcNow.AddSeconds(_appSettings.ExpireTime), // 5-10 
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
                Token = GenerateRefreshToken()
            };

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == claimsCurrentUser.UserId && x.ExpiryDate> DateTime.UtcNow);
            

            await _context.RefreshTokens.AddAsync(refreshToken);
            var res=  await _context.SaveChangesAsync();

            if (res < 0)
                return new ApiResponse { resultCode = Result.ERROR };

            if (storedToken != null)
                return new ApiResponse {resultCode= Result.MULTI_AUTH, token = jwtToken, refreshToken= refreshToken.Token };

            return new ApiResponse { resultCode = Result.SUCCESS , token = jwtToken, refreshToken = refreshToken.Token };

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
                   return new ApiResponse{ resultCode = Result.TOKEN_NOTFOUND };
                

                
                if (storedToken.JwtId != claimsCurrentUser.Jti)
                    return new ApiResponse{ resultCode = Result.TOKEN_NOTMATCH };
                #endregion

                #region refreshToken
                // Generate a new token

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY));


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                   {

                        new Claim(ClaimTypes.Name,claimsCurrentUser.UserName),
                        new Claim(ClaimTypes.NameIdentifier, claimsCurrentUser.UserId),
                        new Claim(ClaimTypes.Role, claimsCurrentUser.Role),
                        //new Claim("LoggedOn", DateTime.Now.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                }),
                    Expires = DateTime.UtcNow.AddSeconds(_appSettings.ExpireTime), // 5-10 
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                var jwtToken = tokenHandler.WriteToken(token);

                // update current token 

                storedToken.JwtId = token.Id;
                storedToken.AddedDate = DateTime.UtcNow;
                storedToken.ExpiryDate = tokenDescriptor.Expires;
                storedToken.Token = GenerateRefreshToken();


                _context.RefreshTokens.Update(storedToken);
                var res = await _context.SaveChangesAsync();
                if (res < 0)
                    return new ApiResponse { resultCode = Result.ERROR };

                return new ApiResponse { resultCode = Result.SUCCESS, token = jwtToken, refreshToken = storedToken.Token };
                #endregion


            }
            catch (Exception ex)
            {

                return new ApiResponse{ resultCode = Result.EXCEPTION  };
                
            }
        }


        #endregion

        #region Helpers
        private string GenerateRefreshToken()
        {
            return Operations.RandomString(5);
        }
       
        #endregion
    }
}