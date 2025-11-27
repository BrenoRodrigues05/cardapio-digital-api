namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// Representa uma resposta genérica retornada pela API.
    /// </summary>
    /// <remarks>
    /// Esta classe é utilizada para padronizar mensagens simples de retorno,
    /// como status de operações e mensagens informativas ou de erro.
    /// </remarks>
    public class Response
    {
        /// <summary>
        /// Status da operação realizada.
        /// </summary>
        /// <example>Sucesso</example>
        /// <example>Erro</example>
        public string? Status { get; set; }

        /// <summary>
        /// Mensagem detalhada sobre o resultado da operação.
        /// </summary>
        /// <example>Usuário autenticado com sucesso.</example>
        public string? Message { get; set; }
    }
}
