using Library.Domain.Models;

namespace Library.Application.Common.Interfaces.Services
{
    public interface ITokenService
    {
        Task<TokensPair> GenerateTokensPairAsync(User user);
        Task RevokeRefreshTokenAsync();
        Task<TokensPair> RefreshTokensAsync(string refreshToken);
        int GetUserIdFromAccessToken();
    }
}
