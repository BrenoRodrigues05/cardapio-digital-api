using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    public class UsuarioCreateDTO
    {
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
        [RegularExpression(@"^\d{11}$|^\d{14}$", ErrorMessage = "CPF/CNPJ inválido.")]
        public string CpfCnpj { get; set; } = string.Empty;

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [MaxLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 100 caracteres.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Confirmação da senha.
        /// </summary>
        [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

}
