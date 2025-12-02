using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    public class FormaPagamentoCreateDTO
    {
        [Required]
        public string Nome { get; set; } = string.Empty;
    }
}
