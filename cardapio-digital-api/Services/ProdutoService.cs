using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;

namespace cardapio_digital_api.Services
{   /// <summary>
    /// Serviço responsável por gerenciar operações relacionadas a produtos.
    /// Inclui CRUD, controle de estoque, disponibilidade e cálculos relacionados.
    /// </summary>
    public class ProdutoService : IProdutoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private ILogger<ProdutoService> _logger;

        /// <summary>
        /// Construtor do serviço de produtos.
        /// </summary>
        /// <param name="logger">Logger para registrar informações e erros.</param>
        /// <param name="unitOfWork">Unit of Work para acesso aos repositórios.</param>
        public ProdutoService(ILogger<ProdutoService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Adiciona uma quantidade ao estoque de um produto existente.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <param name="quantidade">Quantidade a adicionar.</param>
        /// <returns>Retorna true se a operação foi realizada com sucesso.</returns>
        /// <exception cref="ArgumentException">Se o ID ou quantidade forem inválidos.</exception>
        /// <exception cref="KeyNotFoundException">Se o produto não existir.</exception>
        public async Task<bool> AdicionarEstoqueAsync(int id, int quantidade)
        {
            _logger.LogInformation("Adicionando {Quantidade} ao estoque do produto com ID {ProdutoId}", quantidade, id);

            if(id <= 0)
            {
                _logger.LogWarning("ID do produto inválido: {ProdutoId}. O ID deve ser maior que zero.", id);
                throw new ArgumentException("O ID do produto deve ser maior que zero.", nameof(id));
            }

            if (quantidade <= 0)
            {
                _logger.LogWarning("Quantidade inválida: {Quantidade}. A quantidade deve ser maior que zero.", quantidade);
                throw new ArgumentException("A quantidade a ser adicionada deve ser maior que zero.", nameof(quantidade));
            }

            var product = await _unitOfWork.Produtos.GetByIdAsync(id);

            if(product == null)
            {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", id);
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }

            product.QuantidadeEstoque += quantidade;

            await _unitOfWork.Produtos.Update(product);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Novo estoque do produto com ID {ProdutoId} é {NovoEstoque}", id, product.QuantidadeEstoque);

            return true;
        }

        /// <summary>
        /// Altera a disponibilidade de um produto.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <param name="disponibilidade">Novo estado de disponibilidade.</param>
        /// <returns>True se a alteração foi realizada, false se a operação não é necessária.</returns>
        /// <exception cref="ArgumentException">Se o ID for inválido.</exception>
        /// <exception cref="KeyNotFoundException">Se o produto não for encontrado.</exception>
        public async Task<bool> AlterarDisponibilidadeAsync(int id, bool disponibilidade)
        { 
            if(id <= 0)
            {
                _logger.LogWarning("Não existe um produto com ID menor ou igual a zero.");
                throw new ArgumentException("O ID do produto deve ser maior que zero.", nameof(id));
            }
        
            var productavailability = await _unitOfWork.Produtos.GetByIdAsync(id);

           if (productavailability is null)
           {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", id);
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
           }

           if(productavailability.QuantidadeEstoque <= 0)
            {
                _logger.LogInformation("Produto sem estoque. Não é possível alterar disponibilidade. " +
                    "Primeiro adicione um produto ao estoque.");
                return false;
            }

           if(disponibilidade == productavailability.Disponivel)
            {
                _logger.LogWarning("A disponibilidade é igual ao novo valor solicitado"); 
                return false;
            }

            var disponibilidadeAnterior = productavailability.Disponivel;
            productavailability.Disponivel = disponibilidade;

            _logger.LogInformation("Alterando a disponibilidade do produto com ID {id} para {Novo}", id, productavailability.Disponivel);

           await _unitOfWork.Produtos.Update(productavailability);
           await _unitOfWork.CommitAsync();

            return true;

        }

        /// <summary>
        /// Atualiza a quantidade em estoque de um produto.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <param name="quantidade">Nova quantidade em estoque.</param>
        /// <returns>True se o estoque foi atualizado, false se não houver alterações.</returns>
        /// <exception cref="ArgumentException">Se a quantidade for negativa.</exception>
        /// <exception cref="KeyNotFoundException">Se o produto não existir.</exception>
        public async Task<bool> AtualizarEstoqueAsync(int id, int quantidade)
        {
           var product = await _unitOfWork.Produtos.GetByIdAsync(id);

            if(product == null)
            {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", id);
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }

            if (quantidade < 0)
            {
                _logger.LogWarning("Quantidade de estoque inválida: {Quantidade}. O estoque não pode ser negativo.", quantidade);
                throw new ArgumentException("A quantidade de estoque não pode ser negativa.", nameof(quantidade));
            }

            if (product.QuantidadeEstoque == quantidade)
            {
                _logger.LogInformation("Estoque do produto com ID {ProdutoId} já é {Quantidade}. Nenhuma atualização necessária.", id, quantidade);
                return false;
            }

            product.QuantidadeEstoque = quantidade;

            await _unitOfWork.Produtos.Update(product);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Estoque do produto com ID {ProdutoId} atualizado para {NovoEstoque}", id, quantidade);

            return true;
        }

        /// <summary>
        /// Atualiza o preço de um produto.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <param name="novoPreco">Novo preço do produto.</param>
        /// <returns>True se o preço foi atualizado, false se não houver alterações.</returns>
        /// <exception cref="ArgumentException">Se o preço for negativo.</exception>
        /// <exception cref="KeyNotFoundException">Se o produto não existir.</exception>
        public async Task<bool> AtualizarPrecoAsync(int id, decimal novoPreco)
        {
            var product = await _unitOfWork.Produtos.GetByIdAsync(id);

            if (product == null) 
            {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", id);
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }

            if (novoPreco < 0)
            {
                _logger.LogWarning("Novo preço inválido: {NovoPreco}. O preço deve ser maior ou igual a zero.", novoPreco);
                throw new ArgumentException("O novo preço deve ser maior ou igual a zero.", nameof(novoPreco));
            }

            if (product.Preco == novoPreco)
            {
                _logger.LogInformation("Preço do produto com ID {ProdutoId} já é {NovoPreco}. Nenhuma atualização necessária.", id, novoPreco);
                return false;
            }

            novoPreco = Math.Round(novoPreco, 2);
            product.Preco = novoPreco;


            await _unitOfWork.Produtos.Update(product);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Preço do produto com ID {ProdutoId} atualizado para {NovoPreco}", id, novoPreco);

            return true;
        }

        /// <summary>
        /// Atualiza os dados de um produto.
        /// </summary>
        /// <param name="produto">Produto com informações atualizadas.</param>
        /// <returns>Produto atualizado.</returns>
        /// <exception cref="ArgumentException">Se a quantidade de estoque for negativa.</exception>
        /// <exception cref="KeyNotFoundException">Se o produto não existir.</exception>
        public async Task<Produto> AtualizarProdutoAsync(Produto produto)
        {
            var existingProduct = await _unitOfWork.Produtos.GetByIdAsync(produto.Id);

            if (existingProduct == null)
            {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", produto.Id);
                throw new KeyNotFoundException($"Produto com ID {produto.Id} não encontrado.");
            }
            
            if(produto.QuantidadeEstoque < 0)
            {
                _logger.LogWarning("Quantidade de estoque inválida: {QuantidadeEstoque}. A quantidade de estoque não pode ser negativa.", produto.QuantidadeEstoque);
                throw new ArgumentException("A quantidade de estoque não pode ser negativa.", nameof(produto.QuantidadeEstoque));
            }

           await _unitOfWork.Produtos.Update(produto);
              await _unitOfWork.CommitAsync();

              _logger.LogInformation("Produto com ID {ProdutoId} atualizado com sucesso.", produto.Id);

                return produto;
        }

        /// <summary>
        /// Busca produtos pelo nome.
        /// </summary>
        /// <param name="nome">Nome ou parte do nome do produto.</param>
        /// <returns>Lista de produtos encontrados.</returns>
        /// <exception cref="ArgumentException">Se o nome estiver vazio ou nulo.</exception>
        public async Task<IEnumerable<Produto>> BuscarProdutosPorNomeAsync(string nome)
        {
           if (string.IsNullOrWhiteSpace(nome))
           {
                _logger.LogWarning("O nome do produto para busca não pode ser nulo ou vazio.");
                throw new ArgumentException("O nome do produto para busca não pode ser nulo ou vazio.", nameof(nome));
           }
          
           var busca = await _unitOfWork.Produtos.GetByPredicateAsync(p => p.Nome.Contains(nome));

            if (busca == null)
            {
                _logger.LogInformation("Nenhum produto encontrado com o nome contendo: {Nome}", nome);
                return Enumerable.Empty<Produto>();
            }

            _logger.LogInformation("Produtos encontrados com o nome contendo: {Nome}", nome);

           return busca;
        }

        /// <summary>
        /// Calcula o valor total do estoque.
        /// </summary>
        /// <param name="restauranteId">ID do restaurante para filtrar os produtos (opcional).</param>
        /// <returns>Valor total do estoque.</returns>
        public async Task<decimal> CalcularValorTotalEstoqueAsync(int? restauranteId = null)
        {
            _logger.LogInformation("Calculando o valor total do estoque para o restaurante ID: {RestauranteId}", restauranteId.HasValue ? restauranteId.Value.ToString() : "Todos");

            IEnumerable<Produto> produtosTotal;

            if (restauranteId.HasValue)
            {
                produtosTotal = await _unitOfWork.Produtos
                    .GetByPredicateAsync(p => p.RestauranteId == restauranteId.Value);
            }
            else
            {
                produtosTotal = await _unitOfWork.Produtos.GetAllAsync();
            }

            if (produtosTotal == null || !produtosTotal.Any())
            {
                _logger.LogInformation("Nenhum produto encontrado para cálculo do estoque.");
                return 0m;
            }
            var somaTotal = produtosTotal.Sum(p => p.Preco * p.QuantidadeEstoque);

            _logger.LogInformation("Valor total do estoque calculado: {ValorTotal}", somaTotal);

            return somaTotal;
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <param name="produto">Produto a ser criado.</param>
        /// <returns>Produto criado.</returns>
        /// <exception cref="ArgumentNullException">Se o produto for nulo.</exception>
        /// <exception cref="InvalidOperationException">Se já existir um produto com mesmo nome no restaurante.</exception>
        public async Task<Produto> CriarProdutoAsync(Produto produto)
        {
            if(produto == null)
            {
                _logger.LogWarning("O produto fornecido para criação é nulo.");
                throw new ArgumentNullException(nameof(produto), "O produto não pode ser nulo.");
            }

            var produtoExiste = await _unitOfWork.Produtos.GetByPredicateAsync(p => p.Nome == produto.Nome && p.RestauranteId == produto.RestauranteId);

            if(produtoExiste != null && produtoExiste.Any())
            {
                _logger.LogWarning("Já existe um produto com o nome {Nome} para o restaurante ID {RestauranteId}.", produto.Nome, produto.RestauranteId);
                throw new InvalidOperationException($"Já existe um produto com o nome {produto.Nome} para este restaurante.");
            }

            _logger.LogInformation("Criando novo produto: {Nome} para o restaurante ID {RestauranteId}.", produto.Nome, produto.RestauranteId);

            await _unitOfWork.Produtos.AddAsync(produto);
            await _unitOfWork.CommitAsync();

            return produto;
        }

        /// <summary>
        /// Remove um produto do sistema.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>True se o produto foi removido, false se não encontrado.</returns>
        public async Task<bool> DeletarProdutoAsync(int id)
        {
           var produto =  await _unitOfWork.Produtos.GetByIdAsync(id);

            if(produto == null)
            {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", id);
                return false;
            }

            _logger.LogInformation("Deletando produto com ID {ProdutoId}.", id);

             _unitOfWork.Produtos.Remove(produto);
             await _unitOfWork.CommitAsync();

            return true;

        }

        /// <summary>
        /// Obtém um produto com todas as informações relacionadas (completo).
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>Produto completo ou null se não existir.</returns>
        public async Task<Produto?> ObterProdutoCompletoAsync(int id)
        {
            var produto = await _unitOfWork.Produtos.GetProdutoCompletoAsync(id);
           
            if (produto == null)
            {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", id);
                return null;
            }

            _logger.LogInformation("Produto com ID {ProdutoId} obtido com sucesso.", id);

            return produto;
        }

        /// <summary>
        /// Obtém um produto pelo ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>Produto ou null se não encontrado.</returns>
        /// <exception cref="ArgumentException">Se o ID for inválido.</exception>
        public async Task<Produto?> ObterProdutoPorIdAsync(int id)
        {
            if(id <= 0)
            {
                _logger.LogWarning("ID do produto inválido: {ProdutoId}. O ID deve ser maior que zero.", id);
                throw new ArgumentException("O ID do produto deve ser maior que zero.", nameof(id));
            }

            var produto = await _unitOfWork.Produtos.GetByIdAsync(id);

            if (produto == null)
            {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", id);
                return null;
            }

            _logger.LogInformation("Produto com ID {ProdutoId} obtido com sucesso.", id);

            return produto;
        }

        /// <summary>
        /// Obtém produtos com estoque abaixo do limite mínimo.
        /// </summary>
        /// <param name="limiteMinimo">Limite mínimo de estoque.</param>
        /// <returns>Produtos com estoque baixo.</returns>
        /// <exception cref="ArgumentException">Se o limite mínimo for negativo.</exception>
        public async Task<IEnumerable<Produto>> ObterProdutosComEstoqueBaixoAsync(int limiteMinimo = 5)
        {
            if (limiteMinimo < 0)
            {
                _logger.LogWarning("Limite mínimo inválido: {LimiteMinimo}. O limite mínimo não pode ser negativo.", limiteMinimo);
                throw new ArgumentException("O limite mínimo não pode ser negativo.", nameof(limiteMinimo));
            }

            var produtosComEstoqueBaixo = await _unitOfWork.Produtos.GetByPredicateAsync(p => p.QuantidadeEstoque <= limiteMinimo);

            _logger.LogInformation("Produtos com estoque abaixo do limite mínimo {LimiteMinimo} obtidos com sucesso.", limiteMinimo);

            return produtosComEstoqueBaixo;
        }

        /// <summary>
        /// Obtém todos os produtos disponíveis.
        /// </summary>
        /// <returns>Lista de produtos disponíveis.</returns>
        public async Task<IEnumerable<Produto>> ObterProdutosDisponiveisAsync()
        {
           var produtosDisponiveis =  await _unitOfWork.Produtos.GetByPredicateAsync(p => p.Disponivel == true);
            _logger.LogInformation("Produtos disponíveis obtidos com sucesso.");
            return produtosDisponiveis;
        }

        /// <summary>
        /// Obtém produtos disponíveis de um restaurante específico.
        /// </summary>
        /// <param name="restauranteId">ID do restaurante.</param>
        /// <returns>Lista de produtos disponíveis.</returns>
        /// <exception cref="ArgumentException">Se o ID do restaurante for inválido.</exception>
        public async Task<IEnumerable<Produto>> ObterProdutosDisponiveisPorRestauranteAsync(int restauranteId)
        {
           if(restauranteId <= 0)
           {
                _logger.LogWarning("ID do restaurante inválido: {RestauranteId}. O ID deve ser maior que zero.", restauranteId);
                throw new ArgumentException("O ID do restaurante deve ser maior que zero.", nameof(restauranteId));
            }

           var produtosDisponiveis =  await _unitOfWork.Produtos.GetByPredicateAsync(p => p.Disponivel == true && p.RestauranteId == restauranteId);
            _logger.LogInformation("Produtos disponíveis para o restaurante ID {RestauranteId} obtidos com sucesso.", restauranteId);

            if (!produtosDisponiveis.Any())
            {
                _logger.LogInformation("Nenhum produto disponível encontrado para o restaurante ID {RestauranteId}.", restauranteId);
                return Enumerable.Empty<Produto>();
            }

            _logger.LogInformation("Produtos disponíveis para o restaurante ID {RestauranteId} obtidos com sucesso.", restauranteId);

            return  produtosDisponiveis;
        }

        /// <summary>
        /// Obtém produtos indisponíveis.
        /// </summary>
        /// <returns>Lista de produtos indisponíveis.</returns>
        public async Task<IEnumerable<Produto>> ObterProdutosIndisponiveisAsync()
        {
            var produtosIndisponiveis = await _unitOfWork.Produtos.GetByPredicateAsync(p => p.Disponivel == false);

            if (!produtosIndisponiveis.Any())
            {
                _logger.LogInformation("Nenhum produto indisponível encontrado.");
                return Enumerable.Empty<Produto>();
            }

            _logger.LogInformation("Produtos indisponíveis obtidos com sucesso.");
            return produtosIndisponiveis;
        }

        /// <summary>
        /// Obtém produtos dentro de uma faixa de preço.
        /// </summary>
        /// <param name="precoMin">Preço mínimo.</param>
        /// <param name="precoMax">Preço máximo.</param>
        /// <returns>Lista de produtos dentro da faixa de preço.</returns>
        /// <exception cref="ArgumentException">Se preços forem inválidos.</exception>
        public async Task<IEnumerable<Produto>> ObterProdutosPorFaixaDePrecoAsync(decimal precoMin, decimal precoMax)
        {
            if (precoMin < 0 || precoMax < 0)
            {
                _logger.LogWarning("Preços inválidos: precoMin={PrecoMin}, precoMax={PrecoMax}. Valores devem ser positivos.", precoMin, precoMax);
                throw new ArgumentException("Os preços devem ser maiores ou iguais a zero.");
            }

            if (precoMin > precoMax)
            {
                _logger.LogWarning("Faixa de preço inválida: precoMin={PrecoMin} é maior que precoMax={PrecoMax}.", precoMin, precoMax);
                throw new ArgumentException("O preço mínimo não pode ser maior que o preço máximo.");
            }

            var produtosNaFaixa = await _unitOfWork.Produtos.GetByPredicateAsync(p => p.Preco >= precoMin && p.Preco <= precoMax);

            if (!produtosNaFaixa.Any())
            {
                _logger.LogInformation("Nenhum produto encontrado na faixa de preço {PrecoMin} a {PrecoMax}.", precoMin, precoMax);
                return Enumerable.Empty<Produto>();
            }

            _logger.LogInformation("Produtos na faixa de preço {PrecoMin} a {PrecoMax} obtidos com sucesso.", precoMin, precoMax);
            return produtosNaFaixa;
        }

        /// <summary>
        /// Obtém produtos de um restaurante específico.
        /// </summary>
        /// <param name="restauranteId">ID do restaurante.</param>
        /// <returns>Lista de produtos do restaurante.</returns>
        /// <exception cref="ArgumentException">Se o ID do restaurante for inválido.</exception>
        public async Task<IEnumerable<Produto>> ObterProdutosPorRestauranteAsync(int restauranteId)
        {
            if (restauranteId <= 0)
            {
                _logger.LogWarning("ID do restaurante inválido: {RestauranteId}. O ID deve ser maior que zero.", restauranteId);
                throw new ArgumentException("O ID do restaurante deve ser maior que zero.", nameof(restauranteId));
            }

            // Consulta ao repositório
            var produtos = await _unitOfWork.Produtos.GetByPredicateAsync(p => p.RestauranteId == restauranteId);

            // Verifica se encontrou algum produto
            if (!produtos.Any())
            {
                _logger.LogInformation("Nenhum produto encontrado para o restaurante ID {RestauranteId}.", restauranteId);
                return Enumerable.Empty<Produto>();
            }

            _logger.LogInformation("Produtos para o restaurante ID {RestauranteId} obtidos com sucesso.", restauranteId);
            return produtos;
        }

        /// <summary>
        /// Obtém a quantidade em estoque de um produto.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>Quantidade em estoque.</returns>
        /// <exception cref="ArgumentException">Se o ID for inválido.</exception>
        /// <exception cref="KeyNotFoundException">Se o produto não existir.</exception>
        public async Task<int> ObterQuantidadeEstoqueAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID do produto inválido: {ProdutoId}. O ID deve ser maior que zero.", id);
                throw new ArgumentException("O ID do produto deve ser maior que zero.", nameof(id));
            }

            // Busca o produto no repositório
            var produto = await _unitOfWork.Produtos.GetByIdAsync(id);

            if (produto == null)
            {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", id);
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }

            _logger.LogInformation("Quantidade em estoque do produto com ID {ProdutoId} obtida com sucesso: {Quantidade}.", id, produto.QuantidadeEstoque);

            return produto.QuantidadeEstoque;
        }

        /// <summary>
        /// Obtém todos os produtos do sistema.
        /// </summary>
        /// <returns>Lista de todos os produtos.</returns>
        public async Task<IEnumerable<Produto>> ObterTodosProdutosAsync()
        {
            var produtos = await _unitOfWork.Produtos.GetAllAsync();

            if (!produtos.Any())
            {
                _logger.LogInformation("Nenhum produto encontrado no sistema.");
                return Enumerable.Empty<Produto>();
            }

            _logger.LogInformation("Todos os produtos obtidos com sucesso. Total: {TotalProdutos}", produtos.Count());

            return produtos;
        }

        /// <summary>
        /// Obtém a contagem total de produtos disponíveis.
        /// </summary>
        /// <returns>Total de produtos disponíveis.</returns>
        public async Task<int> ObterTotalProdutosDisponiveisAsync()
        {
            var produtosDisponiveis = await _unitOfWork.Produtos.GetAllAsync();

            int totalDisponiveis = produtosDisponiveis?.Count(p => p.Disponivel == true) ?? 0;

            _logger.LogInformation("Total de produtos disponíveis obtido com sucesso: {Total}", totalDisponiveis);

            return totalDisponiveis;
        }

        /// <summary>
        /// Verifica se já existe um produto com determinado nome em um restaurante.
        /// </summary>
        /// <param name="nome">Nome do produto.</param>
        /// <param name="restauranteId">ID do restaurante.</param>
        /// <param name="produtoId">ID do produto a ignorar na verificação (opcional).</param>
        /// <returns>True se o produto existe, false caso contrário.</returns>
        /// <exception cref="ArgumentException">Se parâmetros forem inválidos.</exception>
        public async Task<bool> ProdutoComNomeExisteAsync(string nome, int restauranteId, int? produtoId = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                _logger.LogWarning("Nome do produto inválido para verificação de existência.");
                throw new ArgumentException("O nome do produto não pode ser vazio ou nulo.", nameof(nome));
            }

            if (restauranteId <= 0)
            {
                _logger.LogWarning("ID do restaurante inválido: {RestauranteId}.", restauranteId);
                throw new ArgumentException("O ID do restaurante deve ser maior que zero.", nameof(restauranteId));
            }

            var produtosExistentes = await _unitOfWork.Produtos.GetByPredicateAsync(p =>
                p.Nome == nome &&
                p.RestauranteId == restauranteId &&
                (!produtoId.HasValue || p.Id != produtoId.Value)  // Ignora o produto atual se estiver atualizando
            );

            bool existe = produtosExistentes.Any();

            _logger.LogInformation("Verificação de existência de produto com nome '{Nome}' para restaurante ID {RestauranteId}: {Existe}",
                nome, restauranteId, existe);

            return existe;
        }

        /// <summary>
        /// Verifica se um produto existe pelo ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>True se o produto existir, false caso contrário.</returns>
        /// <exception cref="ArgumentException">Se o ID for inválido.</exception>
        public async Task<bool> ProdutoExisteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID do produto inválido: {ProdutoId}. O ID deve ser maior que zero.", id);
                throw new ArgumentException("O ID do produto deve ser maior que zero.", nameof(id));
            }

            var buscaProduto = await _unitOfWork.Produtos.GetByIdAsync(id);

            bool existe = buscaProduto != null;

            _logger.LogInformation("Verificação de existência do produto com ID {ProdutoId}: {Existe}", id, existe);

            return existe;
        }

        /// <summary>
        /// Remove uma quantidade do estoque de um produto.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <param name="quantidade">Quantidade a remover.</param>
        /// <returns>True se a operação foi realizada com sucesso.</returns>
        /// <exception cref="ArgumentException">Se a quantidade ou ID forem inválidos.</exception>
        /// <exception cref="KeyNotFoundException">Se o produto não existir.</exception>
        /// <exception cref="InvalidOperationException">Se não houver estoque suficiente.</exception>
        public async Task<bool> RemoverEstoqueAsync(int id, int quantidade)
        {
            _logger.LogInformation("Removendo {Quantidade} do estoque do produto com ID {ProdutoId}", quantidade, id);

            var produto = await _unitOfWork.Produtos.GetByIdAsync(id);

            if (produto == null)
            {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", id);
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }

            if (quantidade <= 0)
            {
                _logger.LogWarning("Quantidade inválida: {Quantidade}. A quantidade a ser removida deve ser maior que zero.", quantidade);
                throw new ArgumentException("A quantidade a ser removida deve ser maior que zero.", nameof(quantidade));
            }

            if (produto.QuantidadeEstoque < quantidade)
            {
                _logger.LogWarning("Estoque insuficiente para o produto com ID {ProdutoId}. Estoque atual: {EstoqueAtual}, Quantidade solicitada: {QuantidadeSolicitada}",
                    id, produto.QuantidadeEstoque, quantidade);
                throw new InvalidOperationException("Estoque insuficiente para a remoção solicitada.");
            }

            produto.QuantidadeEstoque -= quantidade;

            await _unitOfWork.Produtos.Update(produto);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Novo estoque do produto com ID {ProdutoId} é {NovoEstoque}", id, produto.QuantidadeEstoque);

            return true;
        }

        /// <summary>
        /// Verifica se um produto está disponível.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>True se disponível, false caso contrário.</returns>
        /// <exception cref="ArgumentException">Se o ID for inválido.</exception>
        /// <exception cref="KeyNotFoundException">Se o produto não existir.</exception>
        public async Task<bool> VerificarDisponibilidadeAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID do produto deve ser maior que zero.", nameof(id));

            _logger.LogInformation("Verificando disponibilidade do produto com ID {ProdutoId}", id);

            var produto = await _unitOfWork.Produtos.GetByIdAsync(id);

            if (produto == null)
            {
                _logger.LogWarning("Produto com ID {ProdutoId} não encontrado.", id);
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }

            _logger.LogInformation("Disponibilidade do produto com ID {ProdutoId} é {Disponivel}", id, produto.Disponivel);

            return produto.Disponivel;
        }

    }
}
