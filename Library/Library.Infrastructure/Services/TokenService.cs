using Library.Application.Common.Interfaces.Services;
using Library.Application.Common.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library.Infrastructure.Services
{
    public class TokenService(IConfiguration configuration, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : ITokenService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<TokensPair> GenerateTokensPairAsync(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
            var role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId);

            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, role.Name)
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpirationMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString().Replace("-", ""),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings["RefreshTokenExpirationDays"]))
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
            await _unitOfWork.CompleteAsync();


            return new TokensPair
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task RevokeRefreshTokenAsync()
        {
            var token = ExtractTokenFromHeader() ?? throw new UnauthorizedAccessException("Token is missing.");
            var refreshToken = _unitOfWork.RefreshTokens.GetByTokenAsync(token).Result;
            if (refreshToken != null)
            {
                await _unitOfWork.RefreshTokens.DeleteAsync(refreshToken);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<TokensPair> RefreshTokensAsync(string token)
        {
            var oldToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(token) ?? throw new UnauthorizedAccessException("Invalid refresh token.");
            var user = await _unitOfWork.Users.GetByIdAsync(oldToken.UserId) ?? throw new UnauthorizedAccessException("User not found.");

            var newTokensPair = await GenerateTokensPairAsync(user);

            await _unitOfWork.RefreshTokens.DeleteAsync(oldToken);
            await _unitOfWork.CompleteAsync();

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = newTokensPair.RefreshToken
            };

            return newTokensPair;
        }

        private string? ExtractTokenFromHeader()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return null;
            }

            if (authorizationHeader.StartsWith("Bearer "))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }
            return null;
        }

        public int GetUserIdFromAccessToken()
        {
            var token = ExtractTokenFromHeader() ?? throw new UnauthorizedAccessException("Token is missing.");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("UserId not found in token.");
            }
            return Convert.ToInt32(userIdClaim.Value);
        }
    }
}