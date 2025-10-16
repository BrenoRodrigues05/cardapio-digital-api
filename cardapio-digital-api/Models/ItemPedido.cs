using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa um item individual dentro de um pedido no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Cada item de pedido está associado a um produto específico,
    /// contendo informações sobre quantidade, preço unitário e subtotal calculado automaticamente.
    /// </remarks>
    public class ItemPedido
    {
        /// <summary>
        /// Identificador único do item do pedido.
        /// </summary>
        /// <example>1</example>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Identificador do pedido ao qual este item pertence.
        /// </summary>
        /// <example>10</example>
        public int PedidoId { get; set; }

        /// <summary>
        /// Pedido associado a este item.
        /// </summary>
        public Pedido Pedido { get; set; } = null!;

        /// <summary>
        /// Identificador do produto vinculado a este item.
        /// </summary>
        /// <example>5</example>
        public int ProdutoId { get; set; }

        /// <summary>
        /// Produto associado a este item de pedido.
        /// </summary>
        public Produto Produto { get; set; } = null!;

        /// <summary>
        /// Quantidade do produto neste pedido.
        /// </summary>
        /// <example>2</example>
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser no mínimo 1.")]
        public int Quantidade { get; set; }

        /// <summary>
        /// Valor unitário do produto no momento do pedido.
        /// </summary>
        /// <example>25.50</example>
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que zero.")]
        public decimal PrecoUnitario { get; set; }

        /// <summary>
        /// Valor total do item (calculado automaticamente como Quantidade * PrecoUnitario).
        /// </summary>
        /// <example>51.00</example>
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }
}
