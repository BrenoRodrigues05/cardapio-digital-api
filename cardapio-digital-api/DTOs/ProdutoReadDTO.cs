namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// DTO utilizado para leitura e exibição das informações de um produto no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Este DTO é retornado em operações de consulta (GET) e contém
    /// as principais informações do produto, incluindo dados do restaurante associado.
    /// </remarks>
    public class ProdutoReadDTO
    {
        /// <summary>
        /// Identificador único do produto.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Nome do produto.
        /// </summary>
        /// <example>Hambúrguer Artesanal</example>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada do produto.
        /// </summary>
        /// <example>Hambúrguer de 200g com queijo, bacon e molho especial.</example>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Preço atual do produto.
        /// </summary>
        /// <example>25.50</example>
        public decimal Preco { get; set; }

        /// <summary>
        /// Indica se o produto está disponível para pedidos.
        /// </summary>
        /// <example>true</example>
        public bool Disponivel { get; set; }

        
    }
}
