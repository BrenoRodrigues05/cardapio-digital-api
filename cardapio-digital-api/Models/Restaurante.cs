using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa um restaurante cadastrado no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// O restaurante oferece produtos e recebe pedidos através da plataforma.
    /// Contém informações de contato, categoria e horário de funcionamento.
    /// </remarks>
    public class Restaurante
    {
        /// <summary>
        /// Identificador único do restaurante.
        /// </summary>
        /// <example>1</example>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do restaurante.
        /// </summary>
        /// <example>Restaurante Sabor Da Casa</example>
        [Required(ErrorMessage = "O nome do restaurante é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Endereço completo do restaurante.
        /// </summary>
        /// <example>Rua das Flores, 123 - São Paulo/SP</example>
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Endereco { get; set; } = string.Empty;

        /// <summary>
        /// Categoria principal do restaurante (ex.: Pizza, Lanches, Japonês).
        /// </summary>
        /// <example>Pizza</example>
        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// Número de telefone para contato do restaurante.
        /// </summary>
        /// <example>(11) 98765-4321</example>
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "O telefone informado não é válido.")]
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Horário de funcionamento do restaurante.
        /// </summary>
        /// <example>10:00 - 22:00</example>
        [Required(ErrorMessage = "O horário de funcionamento é obrigatório.")]
        public string HorarioFuncionamento { get; set; } = string.Empty;

        /// <summary>
        /// Lista de produtos oferecidos pelo restaurante.
        /// </summary>
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();

        /// <summary>
        /// Lista de pedidos realizados neste restaurante.
        /// </summary>
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();


    }
}
