using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O Campo de 'E-Mail' é obrigatório")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O Campo de 'Senha' é obrigatório")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string? Password { get; set; }
    }
}
