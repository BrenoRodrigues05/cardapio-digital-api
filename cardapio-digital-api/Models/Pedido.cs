using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa um pedido realizado no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// O pedido é vinculado a um cliente e a um restaurante,
    /// podendo conter múltiplos itens de produtos.
    /// O valor total é calculado automaticamente com base nos itens.
    /// </remarks>
    public class Pedido
    {
        /// <summary>
        /// Identificador único do pedido.
        /// </summary>
        /// <example>1</example>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Data e hora em que o pedido foi realizado.
        /// </summary>
        /// <example>2025-10-16T18:45:00Z</example>
        public DateTime DataPedido { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Status atual do pedido.
        /// </summary>
        /// <remarks>
        /// Os status possíveis são: <b>Aberto</b>, <b>Em Preparo</b>, <b>Entregue</b> e <b>Cancelado</b>.
        /// </remarks>
        /// <example>Aberto</example>
        [Required]
        public string Status { get; set; } = "Aberto";

        /// <summary>
        /// Identificador do cliente que realizou o pedido.
        /// </summary>
        /// <example>3</example>
        public int ClienteId { get; set; }

        /// <summary>
        /// Cliente associado a este pedido.
        /// </summary>
        public Cliente Cliente { get; set; } = null!;

        /// <summary>
        /// Identificador do restaurante responsável pelo pedido.
        /// </summary>
        /// <example>2</example>
        public int RestauranteId { get; set; }

        /// <summary>
        /// Restaurante associado a este pedido.
        /// </summary>
        public Restaurante Restaurante { get; set; } = null!;

        /// <summary>
        /// Lista de itens que compõem o pedido.
        /// </summary>
        /// <remarks>
        /// Cada item contém informações sobre produto, quantidade e subtotal.
        /// </remarks>
        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

        /// <summary>
        /// Valor total do pedido (calculado automaticamente com base nos itens).
        /// </summary>
        /// <example>87.90</example>
        [Display(Name = "Valor Total")]
        [NotMapped]
        public decimal ValorTotal => Itens.Sum(i => i.Subtotal);
    }
}
