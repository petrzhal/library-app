using Library.Domain.Models;

namespace Library.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<TokensPair> GenerateTokensPairAsync(User user);
        Task RevokeRefreshTokenAsync();
        Task<TokensPair> RefreshTokensAsync(string refreshToken);
        int GetUserIdFromAccessToken();
    }
}
