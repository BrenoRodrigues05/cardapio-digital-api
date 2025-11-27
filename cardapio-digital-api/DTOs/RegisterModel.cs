using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// Modelo utilizado para registrar um novo usuário no sistema.
    /// </summary>
    /// <remarks>
    /// Este DTO contém os dados necessários para criação de uma nova conta,
    /// incluindo informações pessoais, de contato e credenciais de acesso.
    /// </remarks>
    public class RegisterModel
    {
        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        /// <example>João da Silva</example>
        public string? Name { get; set; }

        /// <summary>
        /// CPF ou CNPJ do usuário.
        /// </summary>
        /// <remarks>
        /// Deve conter apenas números.  
        /// CPF deve ter 11 dígitos e CNPJ deve ter 14 dígitos.
        /// </remarks>
        /// <example>12345678901</example>
        [Required(ErrorMessage = "O Campo de 'CPF/CNPJ' é obrigatório")]
        [RegularExpression(@"^\d{11}$|^\d{14}$", ErrorMessage = "CPF/CNPJ inválido.")]
        public string? CpfCnpj { get; set; }

        /// <summary>
        /// Endereço de e-mail para contato e login.
        /// </summary>
        /// <example>usuario@email.com</example>
        [Required(ErrorMessage = "O Campo de 'E-Mail' é obrigatório")]
        public string? Email { get; set; }

        /// <summary>
        /// Senha do usuário utilizada para login.
        /// </summary>
        /// <remarks>
        /// A senha deve ter no mínimo 8 caracteres e no máximo 100.
        /// </remarks>
        /// <example>SenhaForte123</example>
        [Required(ErrorMessage = "O Campo de 'Senha' é obrigatório")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string? Password { get; set; }

        /// <summary>
        /// Confirmação da senha informada.
        /// </summary>
        /// <remarks>
        /// Deve ser idêntica ao campo <c>Password</c>.
        /// </remarks>
        /// <example>SenhaForte123</example>
        [Required(ErrorMessage = "O Campo de 'Confirmação de Senha' é obrigatório")]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string? ConfirmPassword { get; set; }
    }
}
