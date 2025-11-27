using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;

namespace cardapio_digital_api.Services
{
    public class ItemPedidoService : IItemPedidoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ItemPedidoService> _logger;

        public ItemPedidoService(IUnitOfWork unitOfWork, ILogger<ItemPedidoService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

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
