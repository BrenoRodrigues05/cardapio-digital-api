using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        /// <summary>
        /// Busca um usuário pelo e-mail.
        /// </summary>
        Task<Usuario?> GetByEmailAsync(string email);

        /// <summary>
        /// Verifica se um email já está cadastrado.
        /// </summary>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Verifica se um CPF ou CNPJ já está cadastrado.
        /// </summary>
        Task<bool> CpfCnpjExistsAsync(string cpfCnpj);

        /// <summary>
        /// Autentica um usuário pelo e-mail e senha hash.
        /// </summary>
        Task<Usuario?> AuthenticateAsync(string email, string passwordHash);

        /// <summary>
        /// Recupera informações do usuário para exibição no perfil.
        /// </summary>
        Task<Usuario?> GetProfileAsync(int usuarioId);
    }
}
