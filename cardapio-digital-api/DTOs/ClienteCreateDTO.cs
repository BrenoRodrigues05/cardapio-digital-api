using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// DTO utilizado para criação de novos clientes no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Contém apenas os campos necessários para o cadastro de um cliente.
    /// É utilizado nas requisições HTTP POST do endpoint de clientes.
    /// </remarks>
    public class ClienteCreateDTO
    {
        /// <summary>
        /// Nome completo do cliente.
        /// </summary>
        /// <example>João da Silva</example>
        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Endereço de e-mail do cliente.
        /// </summary>
        /// <example>joao.silva@email.com</example>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Número de telefone do cliente, incluindo DDD.
        /// </summary>
        /// <example>(11) 98765-4321</example>
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "O telefone informado não é válido.")]
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Endereço completo do cliente, utilizado para entregas.
        /// </summary>
        /// <example>Rua das Laranjeiras, 123 - São Paulo/SP</example>
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres.")]
        public string Endereco { get; set; } = string.Empty;
    }
}
