using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;

namespace cardapio_digital_api.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(IUnitOfWork unitOfWork, ILogger<ClienteService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<Cliente>> ObterTodosAsync()
        {
            _logger.LogInformation("Obtendo todos os clientes.");

            var clientes = await _unitOfWork.Clientes.GetAllAsync();

            _logger.LogInformation("Total de clientes obtidos: {Count}", clientes.Count());

            return clientes;
        }
        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
           if(id <= 0)
           {
                _logger.LogWarning("ID de cliente inválido: {Id}", id);
                throw new ArgumentException("ID de cliente inválido.", nameof(id));
            }

           var buscaCliente = await _unitOfWork.Clientes.GetByIdAsync(id);

            if(buscaCliente == null)
            {
                _logger.LogWarning("Cliente não encontrado para o ID: {Id}", id);
                return null;    
            }
           
            _logger.LogInformation("Cliente obtido com sucesso para o ID: {Id}", id);

            return buscaCliente;
        }
        public async Task<Cliente> CriarAsync(Cliente cliente)
        {
            if(cliente == null)
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
        public async Task<bool> AtualizarAsync(Cliente cliente)
        {
            if(cliente == null)
            {
                _logger.LogWarning("Tentativa de atualizar um cliente nulo.");
                throw new ArgumentNullException(nameof(cliente), "Cliente não pode ser nulo.");
            }

            var buscaCliente = await _unitOfWork.Clientes.GetByIdAsync(cliente.Id);

            if(buscaCliente == null)
            {
                _logger.LogWarning("Cliente não encontrado para atualização com ID: {Id}", cliente.Id);
                return false;
            }

            // Validação: email já existe
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
            buscaCliente.Endereco = cliente.Endereco;
            
            _logger.LogInformation("Salvando alterações para o cliente com ID: {Id}", cliente.Id);

            await _unitOfWork.Clientes.Update(buscaCliente);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cliente atualizado com sucesso com ID: {Id}", cliente.Id);

            return true;

        }

        public async Task<bool> RemoverAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Tentativa de remover cliente com ID inválido: {Id}", id);
                throw new ArgumentException("ID inválido.", nameof(id));
            }

            var buscaCliente =  await _unitOfWork.Clientes.GetByIdAsync(id);

            if(buscaCliente == null)
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
