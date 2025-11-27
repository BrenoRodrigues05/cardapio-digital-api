using Microsoft.AspNetCore.Identity;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa um usuário do sistema com informações adicionais para autenticação.
    /// </summary>
    /// <remarks>
    /// Esta classe herda de <see cref="IdentityUser"/> e adiciona suporte a tokens de atualização
    /// (RefreshToken) e controle de expiração do mesmo.
    /// </remarks>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Token de atualização associado ao usuário.
        /// </summary>
        /// <example>dGhpcy1pcy1hLXJlZnJlc2gtdG9rZW4tZXhhbXBsZQ==</example>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Data e hora de expiração do token de atualização.
        /// </summary>
        /// <remarks>
        /// Após esta data, o RefreshToken não poderá mais ser utilizado para renovar o AccessToken.
        /// </remarks>
        /// <example>2025-12-31T23:59:59</example>
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
