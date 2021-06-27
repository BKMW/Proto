using Core.Dtos;
using Core.Models.Identity;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<ApiResponse> GenerateTokenAsync(User user, string role);
        Task<ApiResponse> RefreshTokenAsync(string refreshToken, ClaimsCurrentUser claimsCurrentUser);
    }
}