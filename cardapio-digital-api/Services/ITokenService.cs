using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace cardapio_digital_api.Services
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration configuration);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, IConfiguration configuration);
    }
}
