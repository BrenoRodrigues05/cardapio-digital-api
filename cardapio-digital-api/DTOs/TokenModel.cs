namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// Representa os tokens de autenticação retornados após o login.
    /// </summary>
    /// <remarks>
    /// Este modelo é utilizado para enviar ao cliente o token de acesso (JWT)
    /// e o token de atualização, permitindo a renovação de credenciais sem
    /// necessidade de novo login.
    /// </remarks>
    public class TokenModel
    {
        /// <summary>
        /// Token JWT utilizado para autorizar o acesso às rotas protegidas.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Token de atualização utilizado para gerar um novo AccessToken.
        /// </summary>
        /// <example>dGhpcy1pcy1hLXJlZnJlc2gtdG9rZW4tZXhhbXBsZQ==</example>
        public string? RefreshToken { get; set; }
    }
}
