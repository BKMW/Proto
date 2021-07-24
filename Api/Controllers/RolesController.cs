using Application.Constants;
using Application.Dtos;
using Application.Dtos.Roles;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        #region ctor
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityDbContext _context;

        public RolesController(RoleManager<IdentityRole> roleManager,
                        IdentityDbContext context
               )
        {
            _roleManager = roleManager;
            _context = context;

        }
        #endregion

        #region Get All Roles
        [HttpGet]
        public async Task<ApiResponse> Get()
        {
            try
            {
                var roles = await _roleManager.Roles.Select(c=> new ListRolesResponse { Id=c.Id,Name=c.Name}).ToListAsync();

                return new ApiResponse<List<ListRolesResponse>> { resultCode = Result.SUCCESS , data= roles };

            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }
        }
        #endregion

        #region ADD ROLE
        [HttpPost("Add")]
        public async Task<ApiResponse> Add(AddRoleRequest dto)
        {
            if (!ModelState.IsValid)
                return new ApiResponse { resultCode = Result.FORM_INVALID };

            try
            {
                if (await _roleManager.RoleExistsAsync(dto.Name))
                    return new ApiResponse { resultCode = Result.ROLE_NOTFOUND };


                await _roleManager.CreateAsync(new IdentityRole(dto.Name.Trim()));

                return new ApiResponse { resultCode = Result.ROLE_EXISTS };

            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }
        }

        #endregion

        #region Get Permissions By Role
        [HttpGet("GetPermissionsByRole")]
        public async Task<ApiResponse> GetPermissionsByRole(string roleId)
        {

            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return new ApiResponse { resultCode = Result.ROLE_NOTFOUND };

                var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
                var allClaims = Permissions.GenerateAllPermissions();
                //var allPermissions = allClaims.Select(p => new CheckBoxViewModel { DisplayValue = p }).ToList();

                //foreach (var permission in allPermissions)
                //{
                //    if (roleClaims.Any(c => c == permission.DisplayValue))
                //        permission.IsSelected = true;
                //}

                //var viewModel = new PermissionsFormViewModel
                //{
                //    RoleId = roleId,
                //    RoleName = role.Name,
                //    RoleCalims = allPermissions
                //};

                return new ApiResponse { resultCode = Result.SUCCESS };

            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }
        }
        #endregion

        #region Manage Permissions
        [HttpPost("ManagePermissions")]
        public async Task<ApiResponse> ManagePermissions(ManagePermissionsRequest dto)
        {
            if (!ModelState.IsValid)
                return new ApiResponse { resultCode = Result.FORM_INVALID };

            try
            {
                    var role = await _roleManager.FindByIdAsync(dto.RoleId);

                if (role == null)
                    return new ApiResponse { resultCode = Result.ROLE_NOTFOUND };

                    var roleClaims = await _roleManager.GetClaimsAsync(role);

                //var roleClaims1 = _context.RoleClaims.Where(x => x.RoleId == dto.RoleId).ToListAsync();

                foreach (var claim in roleClaims)
                    await _roleManager.RemoveClaimAsync(role, claim);


                foreach (var claim in dto.RoleCalims)
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", claim));

                    return new ApiResponse { resultCode = Result.SUCCESS };
            }
            catch (Exception ex)
            {
                return new ApiResponse { resultCode = Result.EXCEPTION };
            }

            #endregion

        }
    }
}