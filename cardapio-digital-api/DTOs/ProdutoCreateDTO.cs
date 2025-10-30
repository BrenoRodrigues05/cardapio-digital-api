using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// DTO utilizado para a criação de um novo produto no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Este DTO é usado nas operações de criação (POST) e contém
    /// apenas os campos necessários para o cadastro de um produto.
    /// </remarks>
    public class ProdutoCreateDTO
    {
        /// <summary>
        /// Nome do produto a ser cadastrado.
        /// </summary>
        /// <example>Hambúrguer Artesanal</example>
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada do produto.
        /// </summary>
        /// <example>Hambúrguer de 200g com queijo, bacon e molho especial.</example>
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
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
        [Required(ErrorMessage = "O restaurante é obrigatório.")]
        public int RestauranteId { get; set; }
    }
}
