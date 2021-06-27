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
    public class TokenService0 //: ITokenService
    {

        private readonly AppSettings _appSettings;
        private readonly IdentityDbContext _context;
        private readonly UserManager<User> _userManager;

        private readonly TokenValidationParameters _tokenValidationParams;



        public TokenService0(
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
        #region GetTokenAsync
        public async Task<TokenResponse> GenerateTokenAsync(User user, string role)
        {
            int result = Result.SUCCESS;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
               {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Role, role),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                       // new Claim("Company", user.Company),
                        new Claim("LoggedOn", DateTime.Now.ToString()),
                }),
                Expires = DateTime.UtcNow.AddSeconds(_appSettings.ExpireTime), // 5-10 
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = tokenDescriptor.Expires,
                Token = Operations.RandomString(8)
            };

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id && x.ExpiryDate > DateTime.UtcNow);
            if (storedToken != null)
                result = Result.MULTI_AUTH;

            await _context.RefreshTokens.AddAsync(refreshToken);
            var res = await _context.SaveChangesAsync();
            if (res < 0)
                result = Result.ERROR;

            return new TokenResponse { resultCode = result, token = jwtToken, refreshToken = refreshToken.Token };

        }
        #endregion

        #region VerifyAndGenerateToken
        public async Task<TokenResponse> VerifyAndGenerateTokenAsync(string refreshToken, ClaimsCurrentUser claimsCurrentUser)
        {

            try
            {

                //JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();


                // Validation 1 - Validation JWT token format
                //var tokenInVerification = tokenHandler.ValidateToken(tokenRequest.token, _tokenValidationParams, out var validatedToken);

                //// Validation 2 - Validate encryption alg
                //if (validatedToken is JwtSecurityToken jwtSecurityToken)
                //{
                //    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                //    if (result == false)
                //    {
                //        return null;
                //    }
                //}

                //// Validation 3 - validate expiry date
                //var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                //var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                //if (expiryDate < DateTime.UtcNow)
                //{
                //    return new AuthResult()
                //    {
                //        Success = false,
                //        Errors = new List<string>() {
                //            "Token has not yet expired"
                //        }
                //    };
                //}
                // normaly started verifcation from here
                // validation 4 - validate existence of the token


                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

                if (storedToken == null)
                {
                    return new TokenResponse()
                    {
                        resultCode = Result.TOKEN_NOTFOUND

                    };
                }

                // Validation 5 - validate if used
                //if (storedToken.IsUsed)
                //{
                //    return new AuthResult()
                //    {
                //        Success = false,
                //        Errors = new List<string>() {
                //            "Token has been used"
                //        }
                //    };
                //}

                // Validation 6 - validate if revoked
                //if (storedToken.IsRevorked)
                //{
                //    return new AuthResult()
                //    {
                //        Success = false,
                //        Errors = new List<string>() {
                //            "Token has been revoked"
                //        }
                //    };
                //}

                // Validation 7 - validate the id  i think not necessary
                //  var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != claimsCurrentUser.Jti)
                {
                    return new TokenResponse
                    {
                        resultCode = Result.TOKEN_NOTMATCH

                    };

                }


                // Generate a new token

                var jwtToken = await RefreshTokenAsync(storedToken, claimsCurrentUser);
                return jwtToken;
            }
            catch (Exception ex)
            {

                return new TokenResponse
                {
                    resultCode = Result.EXCEPTION
                };
                //if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                //{

                //    return new TokenResponse()
                //    {
                //        Success = false,
                //        Errors = new List<string>() {
                //            "Token has expired please re-login"
                //        }
                //    };

                //}
                //else
                //{
                //    return new TokenResponse()
                //    {
                //        resultCode = Result.EXCEPTION
                //    };
                //}
            }
        }


        #endregion
        #region RefreshTokenAsync
        private async Task<TokenResponse> RefreshTokenAsync(RefreshToken storedToken, ClaimsCurrentUser claimsCurrentUser)
        {
            int result = Result.SUCCESS;
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY));


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
               {
                        //new Claim(ClaimTypes.Name,tokenInVerification.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value),
                        //new Claim(ClaimTypes.NameIdentifier,tokenInVerification.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
                        //new Claim(ClaimTypes.Role, tokenInVerification.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value),
                        new Claim(ClaimTypes.Name,claimsCurrentUser.UserName),
                        new Claim(ClaimTypes.NameIdentifier, claimsCurrentUser.UserId),
                        new Claim(ClaimTypes.Role, claimsCurrentUser.Role),
                        new Claim("LoggedOn", DateTime.Now.ToString()),
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
            storedToken.Token = Operations.RandomString(8);


            _context.RefreshTokens.Update(storedToken);
            var res = await _context.SaveChangesAsync();
            if (res < 0)
                result = Result.ERROR;

            return new TokenResponse { resultCode = result, token = jwtToken, refreshToken = storedToken.Token };

        }
        #endregion
        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }

    }
}