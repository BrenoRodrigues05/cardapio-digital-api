using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// DTO utilizado para a criação de novos itens dentro de um pedido no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Contém os campos necessários para registrar um item associado a um produto
    /// e a um pedido existente. É utilizado em requisições HTTP POST ao criar ou atualizar pedidos.
    /// </remarks>
    public class ItemPedidoCreateDTO
    {
        /// <summary>
        /// Identificador do produto vinculado a este item de pedido.
        /// </summary>
        /// <example>5</example>
        [Required(ErrorMessage = "O identificador do produto é obrigatório.")]
        public int ProdutoId { get; set; }

        /// <summary>
        /// Quantidade do produto incluída no pedido.
        /// </summary>
        /// <example>2</example>
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser no mínimo 1.")]
        public int Quantidade { get; set; }

        /// <summary>
        /// Valor unitário do produto no momento da inclusão no pedido.
        /// </summary>
        /// <remarks>
        /// Este valor deve refletir o preço do produto no momento do pedido,
        /// podendo ser diferente do preço atual do catálogo.
        /// </remarks>
        /// <example>25.50</example>
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que zero.")]
        public decimal PrecoUnitario { get; set; }
    }
}
