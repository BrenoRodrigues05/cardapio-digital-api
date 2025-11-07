using cardapio_digital_api.Models;
using System.Net;
using System.Text.Json;

namespace cardapio_digital_api.Middlewares
{
    /// <summary>
    /// Middleware responsável por capturar e tratar exceções globais na aplicação.
    /// </summary>
    /// <remarks>
    /// Este middleware intercepta todas as exceções não tratadas que ocorrem durante o processamento
    /// das requisições HTTP e retorna uma resposta JSON padronizada contendo informações
    /// sobre o erro. Também realiza o registro de logs detalhados para monitoramento e diagnóstico.
    /// 
    /// O comportamento do retorno pode variar conforme o ambiente configurado:
    /// - Em <b>Desenvolvimento</b>, detalhes da stack trace são incluídos na resposta.
    /// - Em <b>Produção</b>, apenas a mensagem de erro é exibida ao cliente.
    /// </remarks>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="ExceptionMiddleware"/>.
        /// </summary>
        /// <param name="next">Delegate que representa o próximo middleware no pipeline.</param>
        /// <param name="logger">Instância do logger utilizada para registrar os erros capturados.</param>
        /// <param name="env">Provedor de informações sobre o ambiente atual (Desenvolvimento, Produção, etc.).</param>
        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Executa o middleware, capturando exceções lançadas durante o processamento da requisição.
        /// </summary>
        /// <param name="context">O contexto HTTP da requisição atual.</param>
        /// <returns>
        /// Uma tarefa assíncrona que representa a execução do middleware. 
        /// Retorna uma resposta JSON com detalhes do erro caso uma exceção ocorra.
        /// </returns>
        /// <remarks>
        /// - Exceções conhecidas, como <see cref="ArgumentException"/>, <see cref="KeyNotFoundException"/> e 
        ///   <see cref="UnauthorizedAccessException"/>, resultam em códigos de status específicos (400, 404, 401).
        /// - Exceções não tratadas explicitamente retornam o status <c>500 Internal Server Error</c>.
        /// </remarks>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                context.Response.ContentType = "application/json";

                var statusCode = ex switch
                {
                    ArgumentNullException => (int)HttpStatusCode.BadRequest,
                    ArgumentException => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                context.Response.StatusCode = statusCode;

                var response = new ErrorResponse(
                    statusCode,
                    ex.Message,
                    _env.IsDevelopment() ? ex.StackTrace : null
                );

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
