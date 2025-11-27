using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Services
{
    public class RestauranteService : IRestauranteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RestauranteService> _logger;

        public RestauranteService(IUnitOfWork unitOfWork, ILogger<RestauranteService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public Task<IEnumerable<Restaurante>> ObterTodosAsync()
        {
            _logger.LogInformation("Obtendo todos os restaurantes.");

            return _unitOfWork.Restaurantes.GetAllAsync();
        }
        public async Task<Restaurante?> ObterPorIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID inválido fornecido: {Id}", id);
                throw new ArgumentException("ID inválido.", nameof(id));
            }

            var restaurante = await _unitOfWork.Restaurantes.GetByIdAsync(id);

            if(restaurante == null)
            {
                _logger.LogInformation("Restaurante com ID {Id} não encontrado.", id);
                throw new KeyNotFoundException($"Restaurante com ID {id} não encontrado.");
            }
           
            _logger.LogInformation("Restaurante com ID {Id} obtido com sucesso.", id);

            return restaurante;
        }
        public async Task<IEnumerable<Restaurante>> BuscarPorNomeAsync(string nome)
        {
           if (string.IsNullOrWhiteSpace(nome))
            {
                _logger.LogWarning("Nome inválido fornecido para busca.");
                throw new ArgumentException("Nome inválido.", nameof(nome));
            }
           
           var busca = await _unitOfWork.Restaurantes.GetByPredicateAsync(r => r.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));

            if(busca == null)
            {
                _logger.LogInformation("Nenhum restaurante encontrado com o nome: {Nome}", nome);
                throw new KeyNotFoundException($"Nenhum restaurante encontrado com o nome: {nome}");
            }

            _logger.LogInformation("Restaurantes com o nome {Nome} obtidos com sucesso.", nome);

            return busca;
        }

        public async Task<bool> AtualizarAsync(Restaurante restaurante)
        {
            var existente = await _unitOfWork.Restaurantes.GetByIdAsync(restaurante.Id);

            if (existente == null)
            {
                _logger.LogWarning("Restaurante com ID {Id} não encontrado para atualização.", restaurante.Id);
                throw new KeyNotFoundException($"Restaurante com ID {restaurante.Id} não encontrado.");
            }

            _logger.LogInformation("Atualizando restaurante com ID {Id}.", restaurante.Id);

            // Atualiza apenas os campos necessários
            existente.Nome = restaurante.Nome;
            existente.Endereco = restaurante.Endereco;
            existente.Telefone = restaurante.Telefone;
            existente.Categoria = restaurante.Categoria;
            existente.HorarioFuncionamento = restaurante.HorarioFuncionamento;
           
            await _unitOfWork.Restaurantes.Update(restaurante);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Restaurante com ID {Id} atualizado com sucesso.", restaurante.Id);

            return true;
        }

        public async Task<IEnumerable<Restaurante>> BuscarPorEnderecoAsync(string endereco)
        {
            if(string.IsNullOrWhiteSpace(endereco))
            {
                _logger.LogWarning("Endereço inválido fornecido para busca.");
                throw new ArgumentException("Endereço inválido.", nameof(endereco));
            }

            var busca = await _unitOfWork.Restaurantes
                    .GetByPredicateAsync(r => EF.Functions.Like(r.Endereco, $"%{endereco}%"));

            if (!busca.Any())
            {
                _logger.LogInformation("Nenhum restaurante encontrado com o endereço: {Endereco}", endereco);
                throw new KeyNotFoundException($"Nenhum restaurante encontrado com o endereço: {endereco}");
            }

            _logger.LogInformation("Restaurantes com o endereço {Endereco} obtidos com sucesso.", endereco);

            return busca;
        }
        public async Task<Restaurante> CriarAsync(Restaurante restaurante)
        {
           if(string.IsNullOrWhiteSpace(restaurante.Nome))
            {
                _logger.LogWarning("Nome do restaurante não pode ser vazio.");
                throw new ArgumentException("Nome do restaurante não pode ser vazio.", nameof(restaurante.Nome));
            }

            var existentes = await _unitOfWork.Restaurantes
          .GetByPredicateAsync(r => EF.Functions.Like(r.Nome, restaurante.Nome));

            var existente = existentes.FirstOrDefault();

            if (existente != null)
            {
                _logger.LogWarning("Restaurante com o nome {Nome} já existe.", restaurante.Nome);
                throw new InvalidOperationException($"Restaurante com o nome {restaurante.Nome} já existe.");
            }

            await _unitOfWork.Restaurantes.AddAsync(restaurante);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Restaurante {Nome} criado com sucesso.", restaurante.Nome);
            return restaurante;
        }

        public async Task<int> ObterTotalPedidosAsync(int restauranteId)
        {
            if (restauranteId <= 0)
            {
                _logger.LogWarning("ID de restaurante inválido fornecido: {RestauranteId}", restauranteId);
                throw new ArgumentException("ID de restaurante inválido.", nameof(restauranteId));
            }

            var pedidos = await _unitOfWork.Pedidos
                .GetByPredicateAsync(p => p.RestauranteId == restauranteId);

            if(!pedidos.Any())
            {
                _logger.LogInformation("Nenhum pedido encontrado para o restaurante com ID: {RestauranteId}", restauranteId);
                return 0;
            }

            var totalPedidos = (pedidos).Count();

            _logger.LogInformation("Total de {TotalPedidos} pedidos encontrados para o restaurante com ID: {RestauranteId}", totalPedidos, restauranteId);

            return totalPedidos;
        }

        public async Task<int> ObterTotalProdutosAsync(int restauranteId)
        {
            if(restauranteId <= 0)
            {
                _logger.LogWarning("ID de restaurante inválido fornecido: {RestauranteId}", restauranteId);
                throw new ArgumentException("ID de restaurante inválido.", nameof(restauranteId));
            }

            var buscaRestaurante =  await _unitOfWork.Restaurantes.GetByIdAsync(restauranteId);

            if(buscaRestaurante == null)
            {
                _logger.LogInformation("Restaurante com ID {RestauranteId} não encontrado.", restauranteId);
                throw new KeyNotFoundException($"Restaurante com ID {restauranteId} não encontrado.");
            }

            var produtos = await _unitOfWork.Produtos
                .GetByPredicateAsync(p => p.RestauranteId == restauranteId);

            var totalProdutos = produtos.Count();

            _logger.LogInformation("Total de {TotalProdutos} produtos encontrados para o restaurante com ID: {RestauranteId}", totalProdutos, restauranteId);

            return totalProdutos;
        }

        public async Task<bool> RemoverAsync(int id)
        {
           if (id <= 0)
            {
                _logger.LogWarning("ID inválido fornecido para remoção: {Id}", id);
                throw new ArgumentException("ID inválido.", nameof(id));
            }
            
            var restaurante = await _unitOfWork.Restaurantes.GetByIdAsync(id);

            if(restaurante == null)
            {
                _logger.LogInformation("Restaurante com ID {Id} não encontrado para remoção.", id);
                 return false;
            }

            _logger.LogInformation("Removendo restaurante com ID {Id}.", id);

             _unitOfWork.Restaurantes.Remove(restaurante);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Restaurante com ID {Id} removido com sucesso.", id);

            return true;
        }

        public async Task<bool> RestauranteComNomeExisteAsync(string nome, int? restauranteId = null)
        {
            if(string.IsNullOrWhiteSpace(nome))
            {
                _logger.LogWarning("Nome inválido fornecido para verificação de existência.");
                throw new ArgumentException("Nome inválido.", nameof(nome));
            }

            if(restauranteId == 0)
            {
                _logger.LogWarning("ID de restaurante inválido fornecido: {RestauranteId}", restauranteId);
                throw new ArgumentException("ID inválido.", nameof(restauranteId));
            }

            _logger.LogInformation("Verificando existência de restaurante com o nome: {Nome}", nome);

            var restaurantes = await _unitOfWork.Restaurantes.GetByPredicateAsync(
                     r => r.Nome.ToLower() == nome.ToLower() && (restauranteId == null || r.Id != restauranteId.Value));

            if (!restaurantes.Any())
            {
                _logger.LogInformation("Nenhum restaurante encontrado com o nome: {Nome}", nome);
                return false;
            }

            _logger.LogInformation("Restaurante com o nome {Nome} já existe.", nome);

            return true;
        }

        public async Task<bool> RestauranteExisteAsync(int id)
        {
            if(id <= 0)
            {
                _logger.LogWarning("ID inválido fornecido para verificação de existência: {Id}", id);
                throw new ArgumentException("ID inválido.", nameof(id));
            }

            _logger.LogInformation("Verificando existência de restaurante com ID: {Id}", id);

            var restaurante = await _unitOfWork.Restaurantes.GetByIdAsync(id);

            if (restaurante == null)
            {
                _logger.LogInformation("Restaurante com ID {Id} não encontrado.", id);
                return false;
            }

            _logger.LogInformation("Restaurante com ID {Id} existe.", id);

            return true;
        }
    }
}
