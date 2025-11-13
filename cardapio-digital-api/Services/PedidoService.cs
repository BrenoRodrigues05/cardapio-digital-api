using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Implementação do serviço responsável pelas operações de negócio relacionadas à entidade <see cref="Pedido"/>.
    /// </summary>
    /// <remarks>
    /// Aplica regras de negócio para criação de pedidos, consulta de pedidos completos, atualização de status,
    /// listagem de pedidos e deleção de pedidos. Utiliza <see cref="IUnitOfWork"/> para gerenciar transações e repositórios associados.
    /// </remarks>
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="PedidoService"/>.
        /// </summary>
        /// <param name="uow">Instância de <see cref="IUnitOfWork"/> para acesso aos repositórios e commit de transações.</param>
        public PedidoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Cria um novo pedido aplicando todas as regras de negócio necessárias.
        /// </summary>
        /// <param name="pedido">Objeto <see cref="Pedido"/> contendo os dados do pedido a ser criado.</param>
        /// <returns>O identificador único do pedido criado.</returns>
        /// <exception cref="Exception">
        /// Lançada quando o cliente ou restaurante não existe, quando o pedido não contém itens
        /// ou quando há insuficiência de estoque para algum produto.
        /// </exception>
        /// <remarks>
        /// O método debita automaticamente o estoque dos produtos incluídos no pedido,
        /// define o preço unitário de cada item e salva todas as alterações no banco de dados.
        /// </remarks>
        public async Task<int> CriarPedidoAsync(Pedido pedido)
        {
            var client = await _uow.Clientes.GetByIdAsync(pedido.ClienteId);
            if (client == null)
                throw new Exception("Cliente não encontrado");

            var restaurant = await _uow.Restaurantes.GetByIdAsync(pedido.RestauranteId);
            if (restaurant == null)
                throw new Exception("Restaurante não encontrado");

            if (pedido.Itens == null || pedido.Itens.Count == 0)
                throw new Exception("O pedido deve conter pelo menos um item");

            foreach (var item in pedido.Itens)
            {
                var product = await _uow.Produtos.GetByIdAsync(item.ProdutoId);

                if (product == null || product.RestauranteId != pedido.RestauranteId)
                    throw new Exception($"Produto com ID {item.ProdutoId} não encontrado no restaurante");

                if (product.QuantidadeEstoque < item.Quantidade)
                    throw new Exception($"Estoque insuficiente para o produto {product.Nome}");

                product.QuantidadeEstoque -= item.Quantidade;
                _uow.Produtos.Update(product);

                item.PrecoUnitario = product.Preco;
            }

            await _uow.Pedidos.AddAsync(pedido);
            await _uow.CommitAsync();

            return pedido.Id;
        }

        /// <summary>
        /// Recupera um pedido completo com todos os seus relacionamentos carregados.
        /// </summary>
        /// <param name="id">Identificador único do pedido a ser consultado.</param>
        /// <returns>
        /// O <see cref="Pedido"/> completo (Cliente, Restaurante, Itens e Produtos dos itens),
        /// ou <c>null</c> caso o pedido não seja encontrado.
        /// </returns>
        public async Task<Pedido?> GetPedidoCompletoAsync(int id)
        {
            return await _uow.Pedidos.GetPedidoCompletoAsync(id);
        }

        /// <summary>
        /// Atualiza o status de um pedido existente.
        /// </summary>
        /// <param name="id">Identificador único do pedido cujo status será atualizado.</param>
        /// <param name="novoStatus">Novo status a ser atribuído ao pedido.</param>
        /// <returns>
        /// <c>true</c> se a atualização foi realizada com sucesso; <c>false</c> se o pedido não for encontrado.
        /// </returns>
        public async Task<bool> AtualizarStatusPedidoAsync(int id, string novoStatus)
        {
            var order = await _uow.Pedidos.GetByIdAsync(id);
            if (order == null)
                return false;

            order.Status = novoStatus;

            _uow.Pedidos.Update(order);
            await _uow.CommitAsync();

            return true;
        }

        /// <summary>
        /// Recupera todos os pedidos existentes na aplicação.
        /// </summary>
        /// <returns>Uma coleção de <see cref="Pedido"/>.</returns>
        public async Task<IEnumerable<Pedido>> GetAllPedidosAsync()
        {
            return await _uow.Pedidos.GetAllAsync();
        }

        /// <summary>
        /// Recupera todos os pedidos de um cliente específico.
        /// </summary>
        /// <param name="clienteId">Identificador único do cliente.</param>
        /// <returns>Uma coleção de <see cref="Pedido"/> pertencentes ao cliente especificado.</returns>
        public async Task<IEnumerable<Pedido>> GetPedidosPorClienteAsync(int clienteId)
        {
            var all = await _uow.Pedidos.GetAllAsync();
            return all.Where(p => p.ClienteId == clienteId).ToList();
        }

        /// <summary>
        /// Deleta um pedido existente da aplicação.
        /// </summary>
        /// <param name="id">Identificador único do pedido a ser deletado.</param>
        /// <returns>
        /// <c>true</c> se o pedido foi deletado com sucesso, ou <c>false</c> se o pedido não foi encontrado.
        /// </returns>
        public async Task<bool> DeletarPedidoAsync(int id)
        {
            var order = await _uow.Pedidos.GetByIdAsync(id);
            if (order == null) return false;

            _uow.Pedidos.Remove(order);
            await _uow.CommitAsync();
            return true;
        }
    }
}
