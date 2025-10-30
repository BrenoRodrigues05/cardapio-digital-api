namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// DTO utilizado para a leitura e exibição dos dados de um item de pedido
    /// no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Este DTO é retornado em respostas de requisições HTTP GET,
    /// representando as informações detalhadas de um item pertencente a um pedido.
    /// </remarks>
    public class ItemPedidoReadDTO
    {
        /// <summary>
        /// Identificador único do item do pedido.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Identificador do produto associado a este item.
        /// </summary>
        /// <example>5</example>
        public int ProdutoId { get; set; }

        /// <summary>
        /// Nome do produto vinculado a este item de pedido.
        /// </summary>
        /// <example>Pizza de Calabresa</example>
        public string NomeProduto { get; set; } = string.Empty;

        /// <summary>
        /// Quantidade do produto incluída no pedido.
        /// </summary>
        /// <example>2</example>
        public int Quantidade { get; set; }

        /// <summary>
        /// Valor unitário do produto no momento do pedido.
        /// </summary>
        /// <example>25.50</example>
        public decimal PrecoUnitario { get; set; }

        /// <summary>
        /// Valor total do item, calculado como (Quantidade × Preço Unitário).
        /// </summary>
        /// <example>51.00</example>
        public decimal Subtotal { get; set; }
    }
}
