namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa a estrutura padrão de resposta utilizada para retornar informações de erro na API.
    /// </summary>
    /// <remarks>
    /// Esta classe é usada pelo <see cref="Middlewares.ExceptionMiddleware"/> para formatar e
    /// padronizar o retorno de exceções em formato JSON.  
    /// Ela fornece informações essenciais para o cliente entender o tipo de erro ocorrido,
    /// bem como detalhes adicionais (em ambiente de desenvolvimento).
    /// </remarks>
    public class ErrorResponse
    {
        /// <summary>
        /// Código de status HTTP associado ao erro.
        /// </summary>
        /// <example>404</example>
        public int StatusCode { get; set; }

        /// <summary>
        /// Mensagem descritiva do erro ocorrido.
        /// </summary>
        /// <example>Produto não encontrado.</example>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Detalhes técnicos adicionais sobre o erro, como a stack trace.
        /// </summary>
        /// <remarks>
        /// Este campo normalmente é preenchido apenas em ambiente de desenvolvimento
        /// para auxiliar na depuração.  
        /// Em produção, permanece nulo por motivos de segurança.
        /// </remarks>
        /// <example>System.NullReferenceException: Object reference not set to an instance of an object...</example>
        public string? Details { get; set; }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ErrorResponse"/>.
        /// </summary>
        /// <param name="statusCode">Código de status HTTP representando o tipo do erro.</param>
        /// <param name="message">Mensagem descritiva do erro.</param>
        /// <param name="details">Detalhes adicionais do erro (opcional).</param>
        public ErrorResponse(int statusCode, string message, string? details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}
