using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Serviço responsável pelas operações de negócio relacionadas a clientes.
    /// </summary>
    /// <remarks>
    /// Este serviço encapsula a lógica de acesso aos dados de <see cref="Cliente"/>
    /// utilizando <see cref="IUnitOfWork"/> e fornece logs detalhados para auditoria.
    /// </remarks>
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClienteService> _logger;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="ClienteService"/>.
        /// </summary>
        /// <param name="unitOfWork">Instância do <see cref="IUnitOfWork"/> para acesso a repositórios.</param>
        /// <param name="logger">Instância de <see cref="ILogger{T}"/> para logs de informações e erros.</param>
        public ClienteService(IUnitOfWork unitOfWork, ILogger<ClienteService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os clientes cadastrados.
        /// </summary>
        /// <returns>Uma coleção de <see cref="Cliente"/>.</returns>
        public async Task<IEnumerable<Cliente>> ObterTodosAsync()
        {
            _logger.LogInformation("Obtendo todos os clientes.");

            var clientes = await _unitOfWork.Clientes.GetAllAsync();

            _logger.LogInformation("Total de clientes obtidos: {Count}", clientes.Count());

            return clientes;
        }

        /// <summary>
        /// Obtém um cliente pelo seu ID.
        /// </summary>
        /// <param name="id">ID do cliente a ser obtido.</param>
        /// <returns>O <see cref="Cliente"/> correspondente ou <c>null</c> se não encontrado.</returns>
        /// <exception cref="ArgumentException">Se o ID informado for menor ou igual a zero.</exception>
        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID de cliente inválido: {Id}", id);
                throw new ArgumentException("ID de cliente inválido.", nameof(id));
            }

            var buscaCliente = await _unitOfWork.Clientes.GetByIdAsync(id);

            if (buscaCliente == null)
            {
                _logger.LogWarning("Cliente não encontrado para o ID: {Id}", id);
                return null;
            }

            _logger.LogInformation("Cliente obtido com sucesso para o ID: {Id}", id);

            return buscaCliente;
        }

        /// <summary>
        /// Cria um novo cliente no sistema.
        /// </summary>
        /// <param name="cliente">Objeto <see cref="Cliente"/> a ser criado.</param>
        /// <returns>O <see cref="Cliente"/> criado com o ID gerado.</returns>
        /// <exception cref="ArgumentNullException">Se o cliente for nulo.</exception>
        /// <exception cref="InvalidOperationException">Se já existir um cliente com o mesmo e-mail.</exception>
        public async Task<Cliente> CriarAsync(Cliente cliente)
        {
            if (cliente == null)
            {
                _logger.LogWarning("Tentativa de criar um cliente nulo.");
                throw new ArgumentNullException(nameof(cliente), "Cliente não pode ser nulo.");
            }

            var verificarClienteExistente = await _unitOfWork.Clientes.FirstOrDefaultAsync(c => c.Email == cliente.Email);

            if (verificarClienteExistente != null)
            {
                _logger.LogWarning("Cliente com email {Email} já existe.", cliente.Email);
                throw new InvalidOperationException("Cliente com este email já existe.");
            }

            _logger.LogInformation("Email disponível. Adicionando cliente ao repositório.");

            await _unitOfWork.Clientes.AddAsync(cliente);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cliente criado com sucesso com ID: {Id}", cliente.Id);

            return cliente;
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="cliente">Objeto <see cref="Cliente"/> com dados atualizados.</param>
        /// <returns><c>true</c> se a atualização foi realizada com sucesso; caso contrário, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Se o cliente for nulo.</exception>
        /// <exception cref="InvalidOperationException">Se o e-mail informado já estiver em uso por outro cliente.</exception>
        public async Task<bool> AtualizarAsync(Cliente cliente)
        {
            if (cliente == null)
            {
                _logger.LogWarning("Tentativa de atualizar um cliente nulo.");
                throw new ArgumentNullException(nameof(cliente), "Cliente não pode ser nulo.");
            }

            var buscaCliente = await _unitOfWork.Clientes.GetByIdAsync(cliente.Id);

            if (buscaCliente == null)
            {
                _logger.LogWarning("Cliente não encontrado para atualização com ID: {Id}", cliente.Id);
                return false;
            }

            var emailExiste = await _unitOfWork.Clientes
                .FirstOrDefaultAsync(c => c.Email == cliente.Email && c.Id != cliente.Id);

            if (emailExiste != null)
            {
                _logger.LogWarning("Tentativa de atualizar cliente com e-mail já existente: {Email}", cliente.Email);
                throw new InvalidOperationException("Este e-mail já está em uso.");
            }

            _logger.LogInformation("Atualizando cliente com ID: {Id}", cliente.Id);

            buscaCliente.Nome = cliente.Nome;
            buscaCliente.Email = cliente.Email;
            buscaCliente.Telefone = cliente.Telefone;

            _logger.LogInformation("Salvando alterações para o cliente com ID: {Id}", cliente.Id);

            await _unitOfWork.Clientes.Update(buscaCliente);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cliente atualizado com sucesso com ID: {Id}", cliente.Id);

            return true;
        }

        /// <summary>
        /// Remove um cliente pelo seu ID.
        /// </summary>
        /// <param name="id">ID do cliente a ser removido.</param>
        /// <returns><c>true</c> se o cliente foi removido com sucesso; caso contrário, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Se o ID informado for inválido (menor ou igual a zero).</exception>
        public async Task<bool> RemoverAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Tentativa de remover cliente com ID inválido: {Id}", id);
                throw new ArgumentException("ID inválido.", nameof(id));
            }

            var buscaCliente = await _unitOfWork.Clientes.GetByIdAsync(id);

            if (buscaCliente == null)
            {
                _logger.LogWarning("Cliente não encontrado para remoção com ID: {Id}", id);
                return false;
            }

            _logger.LogInformation("Removendo cliente com ID: {Id}", id);

            _unitOfWork.Clientes.Remove(buscaCliente);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cliente removido com sucesso com ID: {Id}", id);

            return true;
        }
    }
}
