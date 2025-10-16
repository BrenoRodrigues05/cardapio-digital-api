using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa um cliente cadastrado no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// O cliente é o usuário que realiza pedidos através da plataforma.
    /// Cada cliente possui informações de contato e endereço,
    /// além de uma lista de pedidos associados.
    /// </remarks>
    public class Cliente
    {
        /// <summary>
        /// Identificador único do cliente.
        /// </summary>
        /// <example>1</example>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do cliente.
        /// </summary>
        /// <example>João da Silva</example>
        [Required(ErrorMessage = "O Nome é Obrigatório!")]
        public string? Nome { get; set; }

        /// <summary>
        /// Endereço de e-mail do cliente.
        /// </summary>
        /// <example>joao.silva@email.com</example>
        [Required(ErrorMessage = "O Email é Obrigatório!")]
        [EmailAddress(ErrorMessage = "O Email informado não é válido!")]
        public string? Email { get; set; }

        /// <summary>
        /// Número de telefone do cliente, incluindo DDD.
        /// </summary>
        /// <example>(11) 98765-4321</example>
        [Required(ErrorMessage = "O Telefone é Obrigatório!")]
        [Phone(ErrorMessage = "O Telefone informado não é válido!")]
        public string? Telefone { get; set; }

        /// <summary>
        /// Endereço completo do cliente, utilizado para entregas.
        /// </summary>
        /// <example>Rua das Laranjeiras, 123 - São Paulo/SP</example>
        [Required(ErrorMessage = "O Endereço é Obrigatório!")]
        public string? Endereco { get; set; }

        /// <summary>
        /// Coleção de pedidos realizados pelo cliente.
        /// </summary>
        /// <remarks>
        /// Cada cliente pode ter um ou mais pedidos registrados.
        /// </remarks>
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
