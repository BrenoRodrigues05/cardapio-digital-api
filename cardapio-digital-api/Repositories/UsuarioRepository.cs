using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Repositório específico para operações com a entidade <see cref="Usuario"/>.
    /// </summary>
    /// <remarks>
    /// Este repositório estende o repositório genérico <see cref="Repository{T}"/> e fornece métodos
    /// específicos de usuário, como autenticação, verificação de existência de email/CPF/CNPJ e
    /// obtenção de perfil.
    /// </remarks>
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly ILogger<UsuarioRepository> _logger;

        /// <summary>
        /// Inicializa uma nova instância do repositório de usuários.
        /// </summary>
        /// <param name="ctx">Contexto do banco de dados.</param>
        /// <param name="logger">Instância de <see cref="ILogger"/> para logs do repositório.</param>
        public UsuarioRepository(CardapioDigitalDbContext ctx, ILogger<UsuarioRepository> logger) : base(ctx)
        {
            _logger = logger;
        }

        /// <summary>
        /// Obtém o perfil de um usuário pelo seu ID.
        /// </summary>
        /// <param name="usuarioId">ID do usuário.</param>
        /// <returns>Retorna o <see cref="Usuario"/> caso encontrado; caso contrário, retorna <c>null</c>.</returns>
        public async Task<Usuario?> GetProfileAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                _logger.LogWarning("Tentativa de buscar usuário com Id inválido: {UsuarioId}", usuarioId);
                return null;
            }

            var buscarUsuario = await GetByIdAsync(usuarioId);

            if (buscarUsuario == null)
            {
                _logger.LogInformation("Nenhum usuário encontrado com o ID: {UsuarioId}", usuarioId);
            }
            else
            {
                _logger.LogInformation("Usuário encontrado com sucesso: {UsuarioId}", usuarioId);
            }

            return buscarUsuario;
        }

        /// <summary>
        /// Busca um usuário pelo seu email.
        /// </summary>
        /// <param name="email">Email do usuário.</param>
        /// <returns>Retorna o <see cref="Usuario"/> caso encontrado; caso contrário, retorna <c>null</c>.</returns>
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Tentativa de buscar usuário com email inválido.");
                return null;
            }

            email = email.Trim().ToLower();
            _logger.LogInformation("Iniciando busca de usuário por email: {Email}", email);

            var usuario = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            if (usuario == null)
            {
                _logger.LogInformation("Nenhum usuário encontrado com o email: {Email}", email);
            }
            else
            {
                _logger.LogInformation("Usuário encontrado com sucesso: {Email}", email);
            }
            return usuario;
        }

        /// <summary>
        /// Autentica um usuário verificando o email e hash da senha.
        /// </summary>
        /// <param name="email">Email do usuário.</param>
        /// <param name="passwordHash">Hash da senha do usuário.</param>
        /// <returns>Retorna o <see cref="Usuario"/> caso as credenciais estejam corretas; caso contrário, <c>null</c>.</returns>
        public async Task<Usuario?> AuthenticateAsync(string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordHash))
            {
                _logger.LogWarning("Tentativa de autenticação com email ou senha inválidos.");
                return null;
            }

            _logger.LogInformation("Iniciando autenticação para o email: {Email}", email);

            var usuario = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.PasswordHash == passwordHash);

            if (usuario == null)
            {
                _logger.LogWarning("Falha na autenticação para email: {Email}", email);
            }
            else
            {
                _logger.LogInformation("Autenticação bem-sucedida para: {Email}", email);
            }

            return usuario;
        }

        /// <summary>
        /// Verifica se um CPF ou CNPJ já está cadastrado no sistema.
        /// </summary>
        /// <param name="cpfCnpj">CPF ou CNPJ a ser verificado.</param>
        /// <returns>Retorna <c>true</c> se já existir; caso contrário, <c>false</c>.</returns>
        public async Task<bool> CpfCnpjExistsAsync(string cpfCnpj)
        {
            if (string.IsNullOrWhiteSpace(cpfCnpj))
            {
                _logger.LogWarning("Tentativa de verificar CPF/CNPJ vazio ou nulo.");
                return false;
            }

            _logger.LogInformation("Verificando existência do CPF/CNPJ: {CpfCnpj}", cpfCnpj);

            var existe = await _dbSet
                .AsNoTracking()
                .AnyAsync(u => u.CpfCnpj == cpfCnpj);

            if (existe)
            {
                _logger.LogInformation("CPF/CNPJ já cadastrado: {CpfCnpj}", cpfCnpj);
            }
            else
            {
                _logger.LogInformation("CPF/CNPJ não encontrado: {CpfCnpj}", cpfCnpj);
            }

            return existe;
        }

        /// <summary>
        /// Verifica se um email já está cadastrado no sistema.
        /// </summary>
        /// <param name="email">Email a ser verificado.</param>
        /// <returns>Retorna <c>true</c> se já existir; caso contrário, <c>false</c>.</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Tentativa de verificar Email vazio ou nulo.");
                return false;
            }

            email = email.Trim().ToLower();

            _logger.LogInformation("Verificando existência do Email: {Email}", email);

            var existe = await _dbSet
                .AsNoTracking()
                .AnyAsync(u => u.Email.ToLower() == email);

            if (existe)
            {
                _logger.LogInformation("Email já cadastrado: {Email}", email);
            }
            else
            {
                _logger.LogInformation("Email não encontrado: {Email}", email);
            }

            return existe;
        }
    }
}
