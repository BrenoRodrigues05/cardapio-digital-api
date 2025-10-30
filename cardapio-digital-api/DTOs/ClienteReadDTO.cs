namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// DTO utilizado para a leitura e exibição dos dados de um cliente no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado em respostas de requisições HTTP GET,
    /// representando as informações de um cliente já cadastrado.
    /// </remarks>
    public class ClienteReadDTO
    {
        /// <summary>
        /// Identificador único do cliente.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do cliente.
        /// </summary>
        /// <example>João da Silva</example>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Endereço de e-mail do cliente.
        /// </summary>
        /// <example>joao.silva@email.com</example>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Número de telefone do cliente, incluindo DDD.
        /// </summary>
        /// <example>(11) 98765-4321</example>
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Endereço completo do cliente.
        /// </summary>
        /// <example>Rua das Laranjeiras, 123 - São Paulo/SP</example>
        public string Endereco { get; set; } = string.Empty;

        /// <summary>
        /// Quantidade total de pedidos realizados pelo cliente.
        /// </summary>
        /// <remarks>
        /// Pode ser utilizada para fins de exibição ou relatórios.
        /// </remarks>
        /// <example>5</example>
        public int TotalPedidos { get; set; }
    }
}
