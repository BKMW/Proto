using Application.Dtos;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        Task<ApiResponse> GenerateTokenAsync(ClaimsCurrentUser claimsCurrentUser);
        Task<ApiResponse> RefreshTokenAsync(string refreshToken, ClaimsCurrentUser claimsCurrentUser);
    }
}