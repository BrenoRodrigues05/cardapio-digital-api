using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Serviço responsável por gerenciar operações relacionadas a itens de pedidos.
    /// </summary>
    /// <remarks>
    /// Fornece métodos para CRUD de itens de pedido, cálculo de subtotal, validação de estoque,
    /// e adição ou incremento de itens em pedidos.
    /// </remarks>
    public class ItemPedidoService : IItemPedidoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ItemPedidoService> _logger;

        /// <summary>
        /// Inicializa uma nova instância do serviço <see cref="ItemPedidoService"/>.
        /// </summary>
        /// <param name="unitOfWork">Instância de <see cref="IUnitOfWork"/> para persistência de dados.</param>
        /// <param name="logger">Instância de <see cref="ILogger{ItemPedidoService}"/> para logging.</param>
        public ItemPedidoService(IUnitOfWork unitOfWork, ILogger<ItemPedidoService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Adiciona um item a um pedido ou incrementa a quantidade caso o item já exista.
        /// </summary>
        /// <param name="pedidoId">ID do pedido.</param>
        /// <param name="produtoId">ID do produto.</param>
        /// <param name="quantidade">Quantidade a ser adicionada.</param>
        /// <param name="precoUnitario">Preço unitário do produto.</param>
        /// <returns>O <see cref="ItemPedido"/> atualizado ou recém-criado.</returns>
        /// <exception cref="ArgumentException">Quando IDs ou quantidade inválidos.</exception>
        /// <exception cref="InvalidOperationException">Quando pedido ou produto inválidos ou indisponíveis.</exception>
        public async Task<ItemPedido> AdicionarOuIncrementarAsync(int pedidoId, int produtoId, int quantidade, decimal precoUnitario)
        {
           if(pedidoId <= 0)
           {
                _logger.LogError("Tentativa de adicionar ou incrementar item com ID de pedido inválido: {PedidoId}", pedidoId);
                throw new ArgumentException("O ID do pedido deve ser maior que zero.", nameof(pedidoId));
            }

           if(produtoId <= 0)
           {
                _logger.LogError("Tentativa de adicionar ou incrementar item com ID de produto inválido: {ProdutoId}", produtoId);
                throw new ArgumentException("O ID do produto deve ser maior que zero.", nameof(produtoId));
            }

           if(quantidade <= 0) {
                _logger.LogError("Tentativa de adicionar ou incrementar item com quantidade inválida: {Quantidade}", quantidade);
                throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));
            }

           if(precoUnitario < 0) {
                _logger.LogError("Tentativa de adicionar ou incrementar item com preço unitário inválido: {PrecoUnitario}", precoUnitario);
                throw new ArgumentException("O preço unitário não pode ser negativo.", nameof(precoUnitario));
            }

            _logger.LogInformation("Adicionando/incrementando produto {ProdutoId} no pedido {PedidoId} - Qtd: {Quantidade}",
         produtoId, pedidoId, quantidade);

            // Verifica se o pedido existe e está em um estado que permite adição/incremento de itens

            var statusPedido = await _unitOfWork.Pedidos.GetPedidoCompletoAsync(pedidoId);

            if (statusPedido == null)
            {
                _logger.LogError("Pedido com ID {PedidoId} não encontrado ao tentar adicionar ou incrementar item.", pedidoId);
                throw new InvalidOperationException($"Pedido com ID {pedidoId} não encontrado.");
            }

            if (statusPedido.Status != "Em Andamento")
            {
                _logger.LogError("Tentativa de adicionar ou incrementar item em pedido com status inválido: {Status} para o pedido {PedidoId}", statusPedido.Status, pedidoId);
                throw new InvalidOperationException($"Não é possível adicionar ou incrementar itens em um pedido com status '{statusPedido.Status}'.");
            }

            // Verifica se o produto existe, está disponível e possui estoque suficiente

            var produtos = await _unitOfWork.Produtos.GetByIdAsync(produtoId);

            if (produtos == null)
            {
                _logger.LogError("Produto com ID {ProdutoId} não encontrado ao tentar adicionar ou incrementar item no pedido {PedidoId}", produtoId, pedidoId);
                throw new InvalidOperationException($"Produto com ID {produtoId} não encontrado.");
            }

            if (produtos.QuantidadeEstoque < quantidade)
            {
                _logger.LogError("Estoque insuficiente para o produto com ID {ProdutoId} ao tentar adicionar ou incrementar item no pedido {PedidoId}. Estoque disponível: {Estoque}, Quantidade solicitada: {Quantidade}", produtoId, pedidoId, produtos.QuantidadeEstoque, quantidade);
                throw new InvalidOperationException($"Estoque insuficiente para o produto com ID {produtoId}. Estoque disponível: {produtos.QuantidadeEstoque}, Quantidade solicitada: {quantidade}");
            }

            if (produtos.Disponivel == false)
            {
                _logger.LogError("Produto com ID {ProdutoId} indisponível ao tentar adicionar ou incrementar item no pedido {PedidoId}", produtoId, pedidoId);
                throw new InvalidOperationException($"Produto com ID {produtoId} está indisponível.");
            }

             await _unitOfWork.ItensPedido.AdicionarOuIncrementarAsync(pedidoId, produtoId, quantidade, precoUnitario);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Item adicionado ou incrementado com sucesso no pedido {PedidoId} para o produto {ProdutoId}", pedidoId, produtoId);

            var pedidoAtualizado = await _unitOfWork.ItensPedido.ObterItemAsync(pedidoId, produtoId);

            if (pedidoAtualizado == null)
            {
                // Isso nunca deve ocorrer, mas deixa o código seguro e remove o warning.
                throw new InvalidOperationException("Erro interno: não foi possível recuperar o item atualizado do pedido.");
            }

            return pedidoAtualizado;
        }

        /// <summary>
        /// Atualiza um item existente do pedido.
        /// </summary>
        /// <param name="itemPedido">Item do pedido a ser atualizado.</param>
        /// <returns><c>true</c> se a atualização foi bem-sucedida.</returns>
        /// <exception cref="InvalidOperationException">Se o item não for encontrado.</exception>
        public async Task<bool> AtualizarAsync(ItemPedido itemPedido)
        {
            _logger.LogInformation("Atualizando item do pedido {ItemPedidoId}", itemPedido.Id);

            var itemExistente = await _unitOfWork.ItensPedido.GetByIdAsync(itemPedido.Id);

            if (itemExistente == null)
            {
                _logger.LogError("Item do pedido com ID {ItemPedidoId} não encontrado para atualização.", itemPedido.Id);
                throw new InvalidOperationException($"Item do pedido com ID {itemPedido.Id} não encontrado.");
            }

            await _unitOfWork.ItensPedido.Update(itemPedido);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Item do pedido {ItemPedidoId} atualizado com sucesso.", itemPedido.Id);

            return true;
        }

        /// <summary>
        /// Calcula o subtotal de todos os itens de um pedido.
        /// </summary>
        /// <param name="pedidoId">ID do pedido.</param>
        /// <returns>O subtotal como <see cref="decimal"/>.</returns>
        /// <exception cref="ArgumentException">Se o ID do pedido for inválido.</exception>
        /// <exception cref="KeyNotFoundException">Se o pedido não existir.</exception>
        /// <exception cref="InvalidOperationException">Se o pedido estiver cancelado.</exception>
        public async Task<decimal> CalcularSubtotalDoPedidoAsync(int pedidoId)
        {
           if(pedidoId <= 0)
           {
                _logger.LogError("Tentativa de calcular subtotal para pedido com ID inválido: {PedidoId}", pedidoId);
                throw new ArgumentException("O ID do pedido deve ser maior que zero.", nameof(pedidoId));
            }

            var pedidoCompleto = await _unitOfWork.Pedidos.GetPedidoCompletoAsync(pedidoId);

            if (pedidoCompleto == null)
            {
                _logger.LogError("Pedido com ID {PedidoId} não encontrado ao tentar calcular subtotal.", pedidoId);
                throw new KeyNotFoundException($"Pedido com ID {pedidoId} não encontrado.");
            }

            // Verifica se há itens no pedido
            if (pedidoCompleto.Itens == null || !pedidoCompleto.Itens.Any())
            {
                _logger.LogInformation("Pedido {PedidoId} não possui itens. Subtotal: 0", pedidoId);
                return 0m;
            }

            if (pedidoCompleto.Status == "Cancelado")
            {
                _logger.LogError("Tentativa de calcular subtotal para pedido cancelado com ID: {PedidoId}", pedidoId);
                throw new InvalidOperationException("Não é possível calcular o subtotal de um pedido cancelado.");
            }

            var subtotal = pedidoCompleto.Itens.Sum(item => item.PrecoUnitario * item.Quantidade);

            _logger.LogInformation("Subtotal calculado com sucesso para o pedido {PedidoId}", pedidoId);

            return subtotal;
        }

        /// <summary>
        /// Cria um novo item de pedido.
        /// </summary>
        /// <param name="itemPedido">Item do pedido a ser criado.</param>
        /// <returns>O <see cref="ItemPedido"/> criado.</returns>
        /// <exception cref="ArgumentNullException">Se o item for nulo.</exception>
        /// <exception cref="InvalidOperationException">Se pedido ou produto não existirem ou estoque insuficiente.</exception>
        public async Task<ItemPedido> CriarAsync(ItemPedido itemPedido)
        {
           if(itemPedido == null)
           {
                _logger.LogError("Tentativa de criar item do pedido com objeto nulo.");
                throw new ArgumentNullException(nameof(itemPedido), "O item do pedido não pode ser nulo.");
            }
            _logger.LogInformation("Criando novo item do pedido para o pedido {PedidoId} e produto {ProdutoId}", itemPedido.PedidoId, itemPedido.ProdutoId);

            if(itemPedido.Quantidade <= 0) {
                _logger.LogError("Tentativa de criar item do pedido com quantidade inválida: {Quantidade}", itemPedido.Quantidade);
                throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(itemPedido.Quantidade));
            }

            // validar se pedido existe
            var pedido = await _unitOfWork.Pedidos.GetByIdAsync(itemPedido.PedidoId);
            if (pedido == null)
            {
                _logger.LogError("Pedido com ID {PedidoId} não encontrado.", itemPedido.PedidoId);
                throw new InvalidOperationException($"Pedido com ID {itemPedido.PedidoId} não encontrado.");
            }

            //validar se produto existe

            var produto = await _unitOfWork.Produtos.GetByIdAsync(itemPedido.ProdutoId);
            if (produto == null)
            {
                _logger.LogError("Produto com ID {ProdutoId} não encontrado.", itemPedido.ProdutoId);
                throw new InvalidOperationException($"Produto com ID {itemPedido.ProdutoId} não encontrado.");
            }

            // validar disponibilidade e estoque

            if (!await ValidarDisponibilidadeEEstoqueAsync(itemPedido.ProdutoId, itemPedido.Quantidade))
            {
                throw new InvalidOperationException("Produto indisponível ou estoque insuficiente.");
            }

            _logger.LogInformation("Adicionando item do pedido ao repositório.");

            await _unitOfWork.ItensPedido.AddAsync(itemPedido);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Item do pedido criado com sucesso com ID {ItemPedidoId}", itemPedido.Id);

            return itemPedido;
        }

        /// <summary>
        /// Obtém todos os itens de um pedido específico.
        /// </summary>
        /// <param name="pedidoId">ID do pedido.</param>
        /// <returns>Uma coleção de <see cref="ItemPedido"/>.</returns>
        /// <exception cref="ArgumentException">Se o ID do pedido for inválido.</exception>
        public async Task<IEnumerable<ItemPedido>> ObterItensPorPedidoAsync(int pedidoId)
        {
            if (pedidoId <= 0)
            {
                _logger.LogError("Tentativa de obter itens de pedido com ID inválido: {PedidoId}", pedidoId);
                throw new ArgumentException("O ID do pedido deve ser maior que zero.", nameof(pedidoId));
            }

            var itensPedido = await _unitOfWork.ItensPedido.GetByPredicateAsync(ip => ip.PedidoId == pedidoId);

            if (!itensPedido.Any())
            {
                _logger.LogInformation("Nenhum item encontrado para o pedido {PedidoId}.", pedidoId);
                return itensPedido; // lista vazia
            }

            _logger.LogInformation("Obtidos {QuantidadeItens} itens para o pedido {PedidoId}.",
                itensPedido.Count(), pedidoId);

            return itensPedido;
        }

        /// <summary>
        /// Obtém um item de pedido pelo seu ID.
        /// </summary>
        /// <param name="id">ID do item do pedido.</param>
        /// <returns>O <see cref="ItemPedido"/> encontrado.</returns>
        /// <exception cref="ArgumentException">Se o ID for inválido.</exception>
        /// <exception cref="KeyNotFoundException">Se o item não for encontrado.</exception>
        public async Task<ItemPedido?> ObterPorIdAsync(int id)
        {
            if(id <= 0)
            {
                _logger.LogError("Tentativa de obter item do pedido com ID inválido: {ItemPedidoId}", id);
                throw new ArgumentException("O ID do item do pedido deve ser maior que zero.", nameof(id));
            }

            var buscaItem =  await _unitOfWork.ItensPedido.GetByIdAsync(id);

            if(buscaItem == null)
            {
                _logger.LogWarning("Item do pedido com ID {ItemPedidoId} não encontrado.", id);
                throw new KeyNotFoundException($"Item do pedido com ID {id} não encontrado.");
            }
            
            _logger.LogInformation("Item do pedido com ID {ItemPedidoId} obtido com sucesso.", id);

            return buscaItem;
        }

        /// <summary>
        /// Remove um item de pedido pelo seu ID.
        /// </summary>
        /// <param name="id">ID do item do pedido.</param>
        /// <returns><c>true</c> se removido com sucesso; caso contrário, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Se o ID for inválido.</exception>
        public async Task<bool> RemoverAsync(int id)
        {
            if(id <= 0)
            {
                _logger.LogError("Tentativa de remover item do pedido com ID inválido: {ItemPedidoId}", id);
                throw new ArgumentException("O ID do item do pedido deve ser maior que zero.", nameof(id));
            }

            var buscaItem = await _unitOfWork.ItensPedido.GetByIdAsync(id);

            if(buscaItem == null)
            {
                _logger.LogError("Item do pedido com ID {ItemPedidoId} não encontrado para remoção.", id);
                return false;
            }   

            _logger.LogInformation("Removendo item do pedido com ID {ItemPedidoId}", id);

             _unitOfWork.ItensPedido.Remove(buscaItem);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Item do pedido com ID {ItemPedidoId} removido com sucesso.", id);

            return true;
        }

        /// <summary>
        /// Valida se um produto está disponível e possui estoque suficiente.
        /// </summary>
        /// <param name="produtoId">ID do produto.</param>
        /// <param name="quantidade">Quantidade desejada.</param>
        /// <returns><c>true</c> se disponível; <c>false</c> caso contrário.</returns>
        /// <exception cref="KeyNotFoundException">Se o produto não for encontrado.</exception>
        public async Task<bool> ValidarDisponibilidadeEEstoqueAsync(int produtoId, int quantidade)
        {
            var buscaProduto = await _unitOfWork.Produtos.GetByIdAsync(produtoId);

            if(buscaProduto == null)
            {
                _logger.LogError("Produto com ID {ProdutoId} não encontrado para validação de disponibilidade e estoque.", produtoId);
                throw new KeyNotFoundException($"Produto com ID {produtoId} não encontrado.");
            }

            if(buscaProduto.Disponivel == false)
            {
                _logger.LogWarning("Produto com ID {ProdutoId} está indisponível.", produtoId);
                return false;
            }

            if(buscaProduto.QuantidadeEstoque < quantidade)
            {
                _logger.LogWarning("Estoque insuficiente para o produto com ID {ProdutoId}. Estoque disponível: {Estoque}, Quantidade solicitada: {Quantidade}", produtoId, buscaProduto.QuantidadeEstoque, quantidade);
                return false;
            }

            _logger.LogInformation("Produto com ID {ProdutoId} está disponível e possui estoque suficiente.", produtoId);

            return true;
        }
    }
}
