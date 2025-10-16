using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa um produto oferecido por um restaurante na plataforma Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Cada produto possui nome, descrição, preço e status de disponibilidade.
    /// Pode ser associado a múltiplos itens de pedidos realizados pelos clientes.
    /// </remarks>
    public class Produto
    {
        /// <summary>
        /// Identificador único do produto.
        /// </summary>
        /// <example>1</example>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do produto.
        /// </summary>
        /// <example>Hambúrguer Artesanal</example>
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada do produto.
        /// </summary>
        /// <example>Hambúrguer de 200g com queijo, bacon e molho especial.</example>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Preço do produto.
        /// </summary>
        /// <example>25.50</example>
        [Required(ErrorMessage = "O preço do produto é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        /// <summary>
        /// Indica se o produto está disponível para pedidos.
        /// </summary>
        /// <example>true</example>
        public bool Disponivel { get; set; } = true;

        /// <summary>
        /// Identificador do restaurante que oferece o produto.
        /// </summary>
        /// <example>2</example>
        public int RestauranteId { get; set; }

        /// <summary>
        /// Restaurante associado ao produto.
        /// </summary>
        public Restaurante? Restaurante { get; set; } = null!;

        /// <summary>
        /// Coleção de itens de pedidos que contêm este produto.
        /// </summary>
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}
