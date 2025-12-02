using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa um entregador responsável por realizar as entregas de pedidos.
    /// </summary>
    public class Entregador
    {
        /// <summary>
        /// Identificador único do entregador.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do entregador.
        /// </summary>
        [Required(ErrorMessage = "O nome do entregador é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome do entregador não pode exceder 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Número de telefone do entregador.
        /// O formato deve corresponder a um número válido.
        /// </summary>
        [Required(ErrorMessage = "O telefone do entregador é obrigatório.")]
        [Phone(ErrorMessage = "O telefone do entregador deve ser um número de telefone válido.")]
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Lista de pedidos que o entregador já realizou.
        /// </summary>
        public ICollection<Pedido> PedidosEntregues { get; set; } = new List<Pedido>();

        /// <summary>
        /// Avaliações recebidas pelos clientes relacionadas ao serviço de entrega.
        /// </summary>
        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    }
}
