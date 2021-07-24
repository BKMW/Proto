using Application.Constants;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Identity.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        #region ctor
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IdentityDbContext context,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _tokenService = tokenService;
        }
        #endregion

        #region RefreshToken  exemple use VerifyAndGenerateTokenAsync   
        [HttpPost]
        [Route("RefreshToken")]
        [AllowAnonymous]
        public async Task<ApiResponse> RefreshToken()
        {
                Request.Headers.TryGetValue("RefreshToken", out var refreshToken);
                var claimsUser = User.GetCurrentUser();
                var token = await _tokenService.RefreshTokenAsync(refreshToken, claimsUser);
              
                if (token.resultCode != Result.SUCCESS)
                    return new ApiResponse { resultCode = Result.TOKEN_INVALID};

                return token;
          
        }
        #endregion

        #region Disconnect from all devices


        #endregion

       
        #region login

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        //POST : /api/Account/Login
        public async Task<ApiResponse> Login([FromBody] LoginRequest dto)
        {

            try
            {
                User user = await _userManager.FindByNameAsync(dto.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
                {

                    if (!user.LockoutEnabled)
                        return new ApiResponse { resultCode = Result.ACCOUNT_DISABLED };

                    if (!user.EmailConfirmed)
                        user.EmailConfirmed = true;

                    user.LastConnection = DateTime.Now;
                    await _userManager.UpdateAsync(user);

                    var roles = await _userManager.GetRolesAsync(user);
                    var claimsCurrentUser = new ClaimsCurrentUser {
                        UserId = user.Id, 
                        UserName = user.UserName, 
                        Role = roles.FirstOrDefault() 
                    };
                    var token = await _tokenService.GenerateTokenAsync(claimsCurrentUser);


                    return new ApiResponse<LoginResponse>
                    {

                        resultCode = token.resultCode,
                        token = token.token,
                        refreshToken= token.refreshToken,
                        data = new LoginResponse
                        {
                            userName = user.UserName,
                            Company = user.Company,
                            lastConnection = user.LastConnection,
                            role = roles.FirstOrDefault()

                        }
                    };
                }

                return new ApiResponse { resultCode = Result.FAILED_AUTH };
            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }
        }
        #endregion

        #region register
        [HttpPost]
        [Route("Register")]
        //POST : /api/Account/Register
        //[Authorize(Roles = "ADMIN")]
        public async Task<ApiResponse> Register([FromBody] RegisterRequest dto)
        {

            var _user = new User
            {
                UserName = dto?.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Company = dto.Company,
                Addeddate = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };


            try
            {
                //dto.Password = GenerateRandomPassword();//GenerateRandomPassword();
                var Password = "Azerty123+";
                var result = await _userManager.CreateAsync(_user, Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(_user, dto.Role);

                    //try
                    //{
                    //    var url = _appSettings.UrlLogin;
                    //    var message = "<div style=''>Bonjour <strong>" + _user.UserName + "</strong>," + " <p> Bienvenue sur la plateforme de MPAYMENt , un compte associé à votre adresse mail a été crée.</p> <p> Votre nom d'utilisateur est : <strong>" + _user.UserName + " </strong><br> Votre mot de passe est : <strong>" + Password + "</strong></p>" + " <p><a href='" + url + "'>Cliquez ici pour login</a>.</p></div>";
                    //    var subject = "Activation de votre compte ";
                    //    // SendEmail(applicationUser.Email, message, subject);

                    //}
                    //catch (Exception ex)
                    //{
                    //    Log.Fatal("Exception : " + ex);
                    //    return Ok(new ApiResponse { result=Result.FAILED_SEND_EMAIL });
                    //}
                    // end send mail
                    return new ApiResponse { resultCode = Result.SUCCESS };
                }
                else
                {
                    List<string> errors = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        errors.Add(error.Description);
                    }
                    return new ApiResponse { resultCode = Result.ERROR };


                }

                //return Ok(result);
            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }
            // }
        }
        #endregion

        #region profile
        [HttpGet]
        [Route("Profile")]
        //[Authorize]
        //POST : /api/Account/Profile
        public ApiResponse Profile()
        {
            try
            {
                var userName = User.GetUserName();
                if (userName == null)
                    return new ApiResponse { resultCode = Result.EXPIRED_TOKEN };

                var user = _userManager.Users
                     .Select(c => new UserDetailsResponse
                     {
                         UserName = c.UserName,
                         FirstName = c.FirstName,
                         LastName = c.LastName,
                         Email = c.Email,
                         PhoneNumber = c.PhoneNumber,
                     })
                    .Where(c => c.UserName == userName)
                    .FirstOrDefault();

                return new ApiResponse<UserDetailsResponse> { resultCode = Result.SUCCESS, data = user };
            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };

            }


        }
        #endregion

        #region delete
        [HttpGet]
        [Route("delete/{id}")]
        //[Authorize(Roles = "ADMIN")]
        //POST : /api/Account/delete
        public async Task<ApiResponse> Delete(string id)
        {
            if (id == null)
                return new ApiResponse { resultCode = Result.INPUT_ERROR };

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return new ApiResponse { resultCode = Result.EXPIRED_TOKEN };
            try
            {
                var res = _userManager.DeleteAsync(user);
                return new ApiResponse { resultCode = Result.SUCCESS };

            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }


        }
        #endregion

        #region ChangePassword
        [HttpPost]
        [Route("ChangePassword")]
        //[Authorize]
        public async Task<ApiResponse> ChangePassword(ChangePasswordRequest dto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.GetUserName());
                if (user == null)
                    return new ApiResponse { resultCode = Result.EXPIRED_TOKEN };

                if (await _userManager.CheckPasswordAsync(user, dto.OldPassword))
                {
                    // compute the new hash string
                    var newPassword = _userManager.PasswordHasher.HashPassword(user, dto.NewPassword);
                    user.PasswordHash = newPassword;
                    var res = await _userManager.UpdateAsync(user);
                    if (res.Succeeded)
                        return new ApiResponse { resultCode = Result.SUCCESS };
                    else
                        return new ApiResponse { resultCode = Result.ERROR };
                }
                return new ApiResponse { resultCode = Result.PWD_ERROR };
            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }


        }
        #endregion

        #region UpdateProfile
        [HttpPost]
        [Route("UpdateProfile")]
        [Authorize]
        public async Task<ApiResponse> UpdateProfile(UpdateProfileRequest dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(User.GetUserId());
                if (user == null)
                    return new ApiResponse { resultCode = Result.EXPIRED_TOKEN };

                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PhoneNumber = dto.PhoneNumber;
                user.Email = dto.Email;
                var res = await _userManager.UpdateAsync(user);
                if (res.Succeeded)
                {
                    return new ApiResponse { resultCode = Result.SUCCESS };
                }
                else
                {
                    return new ApiResponse { resultCode = Result.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }


        }
        #endregion

        #region UpdateUser
        [HttpPost]
        [Route("UpdateUser/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ApiResponse> UpdateUser(string id, UpdateProfileRequest dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return new ApiResponse { resultCode = Result.EXPIRED_TOKEN };


                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PhoneNumber = dto.PhoneNumber;
                user.Email = dto.Email;
                var res = await _userManager.UpdateAsync(user);
                if (res.Succeeded)
                    return new ApiResponse { resultCode = Result.SUCCESS };
                else
                    return new ApiResponse { resultCode = Result.ERROR };
            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }
        }
        #endregion

        #region Lockout

        [HttpPost]
        [Route("Lockout")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ApiResponse> Lockout(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return new ApiResponse { resultCode = Result.EXPIRED_TOKEN };


                user.LockoutEnabled = !user.LockoutEnabled;

                user.AccessFailedCount = 0;

                await _userManager.UpdateAsync(user);
                return new ApiResponse { resultCode = Result.SUCCESS };
            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }


        }
        #endregion

        #region AllUsers

        //[HttpPost]
        //[Route("UsersList")]
        //[Obsolete]
        //public ApiResponse<List<UserListResponseDto>, UserListInfoDto> UsersList(UserListDto dto)
        //{

        //    try
        //    {
        //        //  var test =  _context.Database.ExecuteSqlCommand("test");

        //        var data = _context.SuiviUsers.FromSqlRaw("SuiviUsers @p0,@p1,@p2,@p3,@p4", parameters: new[]{
        //                                                        dto.page,
        //                                                        dto.size,
        //                                                        dto.bank,
        //                                                        dto.userName,
        //                                                        dto.role
        //                                            }).ToList();

        //        var info = _context.InfoUsers.FromSqlRaw("InfoUsers {0},{1},{2}",
        //                                            dto.bank,
        //                                            dto.userName,
        //                                            dto.role
        //                                            ).AsEnumerable().FirstOrDefault();




        //        return new ApiResponse<List<UserListResponseDto>, UserListInfoDto> { resultCode = Result.SUCCESS, data = data, info = info };
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Fatal("Exception : " + ex);
        //        return new ApiResponse<List<UserListResponseDto>, UserListInfoDto> { resultCode = Result.EXCEPTION };
        //    }
        //}


        #endregion
    }
}
