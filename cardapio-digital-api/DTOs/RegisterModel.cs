using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    public class RegisterModel
    {
        public string? Name { get; set; }

        [Required(ErrorMessage = "O Campo de 'CPF/CNPJ' é obrigatório")]
        [RegularExpression(@"^\d{11}$|^\d{14}$", ErrorMessage = "CPF/CNPJ inválido.")]
        public string? CpfCnpj { get; set; }

        [Required(ErrorMessage = "O Campo de 'E-Mail' é obrigatório")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O Campo de 'Senha' é obrigatório")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "O Campo de 'Confirmação de Senha' é obrigatório")]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string? ConfirmPassword { get; set; }
    }
}
