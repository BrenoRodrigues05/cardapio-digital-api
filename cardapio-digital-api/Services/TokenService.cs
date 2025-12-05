using cardapio_digital_api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Serviço responsável pela geração, validação e manipulação de tokens JWT e Refresh Tokens.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Construtor do <see cref="TokenService"/>.
        /// </summary>
        /// <param name="config">Configuração da aplicação (<see cref="IConfiguration"/>), incluindo informações de JWT.</param>
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Usuario usuario)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Name),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            return GenerateAccessToken(claims);
        }

        /// <summary>
        /// Gera um token de acesso (JWT) com base em claims fornecidas.
        /// </summary>
        /// <param name="claims">Coleção de <see cref="Claim"/> que será incluída no token.</param>
        /// <returns>Um <see cref="JwtSecurityToken"/> válido.</returns>
        /// <exception cref="ArgumentNullException">Se a chave secreta não estiver configurada.</exception>
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = _config["Jwt:SecretKey"] ?? throw new ArgumentNullException("SecretKey inválida");

            var privateKey = Encoding.UTF8.GetBytes(key);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(
                    double.Parse(_config["Jwt:TokenValidityInMinutes"] ?? "10")),
                Audience = _config["Jwt:ValidAudience"],
                Issuer = _config["Jwt:ValidIssuer"],
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Gera um refresh token seguro, codificado em Base64.
        /// </summary>
        /// <returns>Refresh token como <see cref="string"/>.</returns>
        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(secureRandomBytes);

            return Convert.ToBase64String(secureRandomBytes);
        }

        /// <summary>
        /// Obtém o <see cref="ClaimsPrincipal"/> a partir de um token expirado, sem validar a expiração.
        /// </summary>
        /// <param name="token">Token JWT expirado.</param>
        /// <param name="configuration">Configuração opcional (não utilizada, o serviço usa a configuração injetada).</param>
        /// <returns>O <see cref="ClaimsPrincipal"/> contido no token.</returns>
        /// <exception cref="ArgumentNullException">Se a chave secreta não estiver configurada.</exception>
        /// <exception cref="SecurityTokenException">Se o token for inválido ou não usar HMAC SHA256.</exception>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, IConfiguration configuration)
        {
            var secretKey = _config["Jwt:SecretKey"] ?? throw new ArgumentNullException("SecretKey inválida");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false // Ignora expiração para refresh token
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido");
            }

            return principal;
        }
    }
}
