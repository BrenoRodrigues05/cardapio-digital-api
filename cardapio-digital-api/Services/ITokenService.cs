using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Interface que define métodos para geração e validação de tokens JWT.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Gera um token de acesso JWT baseado em claims fornecidas.
        /// </summary>
        /// <param name="claims">Coleção de <see cref="Claim"/> que serão incluídas no token.</param>
        /// <param name="configuration">Instância de <see cref="IConfiguration"/> para acessar configurações do token.</param>
        /// <returns>Um <see cref="JwtSecurityToken"/> contendo as claims e expiração configuradas.</returns>
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration configuration);

        /// <summary>
        /// Gera um token de atualização (refresh token) seguro.
        /// </summary>
        /// <returns>Uma string representando o refresh token.</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Obtém o <see cref="ClaimsPrincipal"/> a partir de um token expirado.
        /// </summary>
        /// <param name="token">O token JWT expirado.</param>
        /// <param name="configuration">Instância de <see cref="IConfiguration"/> para validações.</param>
        /// <returns>O <see cref="ClaimsPrincipal"/> extraído do token expirado ou <c>null</c> se inválido.</returns>
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, IConfiguration configuration);
    }
}
