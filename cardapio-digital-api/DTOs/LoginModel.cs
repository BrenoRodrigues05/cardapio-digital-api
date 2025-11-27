using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// Modelo usado para realizar login no sistema.
    /// </summary>
    /// <remarks>
    /// Esta classe representa os dados necessários para autenticação,
    /// contendo o e-mail e a senha do usuário.
    /// </remarks>
    public class LoginModel
    {
        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        /// <example>usuario@email.com</example>
        [Required(ErrorMessage = "O Campo de 'E-Mail' é obrigatório")]
        public string? Email { get; set; }

        /// <summary>
        /// Senha de acesso do usuário.
        /// </summary>
        /// <remarks>
        /// A senha deve possuir no mínimo 8 caracteres.
        /// </remarks>
        /// <example>MinhaSenha123</example>
        [Required(ErrorMessage = "O Campo de 'Senha' é obrigatório")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string? Password { get; set; }
    }
}
