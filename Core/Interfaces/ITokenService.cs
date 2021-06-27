using Core.Dtos;
using Core.Models.Identity;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<ApiResponse> GenerateTokenAsync(ClaimsCurrentUser claimsCurrentUser);
        Task<ApiResponse> RefreshTokenAsync(string refreshToken, ClaimsCurrentUser claimsCurrentUser);
    }
}