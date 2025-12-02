using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa uma forma de pagamento disponível para os pedidos.
    /// </summary>
    public class FormaPagamento
    {
        /// <summary>
        /// Identificador único da forma de pagamento.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome da forma de pagamento (ex.: Cartão, Pix, Dinheiro).
        /// </summary>
        [Required(ErrorMessage = "O nome da forma de pagamento é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Pedidos associados que utilizaram esta forma de pagamento.
        /// </summary>
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
