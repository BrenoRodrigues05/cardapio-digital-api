using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa um usuário do sistema, que pode ser cliente ou administrador.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único do usuário.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// CPF ou CNPJ do usuário.
        /// </summary>
        [Required(ErrorMessage = "O CPF/CNPJ é obrigatório.")]
        [RegularExpression(@"^\d{11}|\d{14}$", ErrorMessage = "CPF deve ter 11 dígitos ou CNPJ 14 dígitos.")]
        public string CpfCnpj { get; set; } = string.Empty;

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email deve ser válido.")]
        [MaxLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash da senha do usuário.
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [MaxLength(255, ErrorMessage = "A senha não pode exceder 255 caracteres.")]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
