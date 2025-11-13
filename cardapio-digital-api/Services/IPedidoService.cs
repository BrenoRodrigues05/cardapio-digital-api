using cardapio_digital_api.Models;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Serviço responsável pelas operações de negócio relacionadas à entidade <see cref="Pedido"/>.
    /// </summary>
    /// <remarks>
    /// Define métodos para criar pedidos, consultar pedidos completos, atualizar status
    /// e deletar pedidos, aplicando regras de negócio específicas da aplicação.
    /// </remarks>
    public interface IPedidoService
    {
        /// <summary>
        /// Recupera todos os pedidos existentes na aplicação.
        /// </summary>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> contendo uma coleção de <see cref="Pedido"/>.
        /// </returns>
        Task<IEnumerable<Pedido>> GetAllPedidosAsync();

        /// <summary>
        /// Recupera todos os pedidos de um cliente específico.
        /// </summary>
        /// <param name="clienteId">Identificador único do cliente.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> contendo uma coleção de <see cref="Pedido"/>
        /// pertencentes ao cliente especificado.
        /// </returns>
        Task<IEnumerable<Pedido>> GetPedidosPorClienteAsync(int clienteId);

        /// <summary>
        /// Cria um novo pedido na aplicação.
        /// </summary>
        /// <param name="pedido">Objeto <see cref="Pedido"/> contendo os dados do pedido a ser criado.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> que, ao completar, retorna o identificador
        /// único do pedido criado no banco de dados.
        /// </returns>
        /// <remarks>
        /// Este método deve aplicar todas as regras de negócio necessárias antes de salvar
        /// o pedido, como validação de disponibilidade de produtos ou verificação de dados do cliente.
        /// </remarks>
        Task<int> CriarPedidoAsync(Pedido pedido);

        /// <summary>
        /// Recupera um pedido completo, com todos os seus relacionamentos carregados.
        /// </summary>
        /// <param name="id">Identificador único do pedido a ser consultado.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> contendo o <see cref="Pedido"/> com seus relacionamentos
        /// carregados (Cliente, Restaurante, Itens e Produtos), ou <c>null</c> se não for encontrado.
        /// </returns>
        Task<Pedido?> GetPedidoCompletoAsync(int id);

        /// <summary>
        /// Atualiza o status de um pedido existente.
        /// </summary>
        /// <param name="id">Identificador único do pedido cujo status será atualizado.</param>
        /// <param name="novoStatus">Novo status a ser atribuído ao pedido.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> que, ao completar, retorna <c>true</c> se
        /// o status foi atualizado com sucesso, ou <c>false</c> caso o pedido não exista
        /// ou a atualização não seja permitida.
        /// </returns>
        /// <remarks>
        /// Este método deve validar a transição de status de acordo com as regras de negócio
        /// (por exemplo, um pedido só pode ir de "Em andamento" para "Finalizado").
        /// </remarks>
        Task<bool> AtualizarStatusPedidoAsync(int id, string novoStatus);

        /// <summary>
        /// Deleta um pedido existente da aplicação.
        /// </summary>
        /// <param name="id">Identificador único do pedido a ser deletado.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> que, ao completar, retorna <c>true</c> se
        /// o pedido foi deletado com sucesso, ou <c>false</c> se o pedido não foi encontrado.
        /// </returns>
        /// <remarks>
        /// A deleção deve respeitar as regras de negócio, como impedir remoção de pedidos
        /// já finalizados ou pagos, dependendo da implementação.
        /// </remarks>
        Task<bool> DeletarPedidoAsync(int id);
    }
}
