using cardapio_digital_api.Models;

namespace cardapio_digital_api.Services
{
    /// <summary>
    /// Serviço responsável pelas operações de negócio relacionadas à entidade <see cref="Pedido"/>.
    /// </summary>
    /// <remarks>
    /// Define métodos para criar pedidos, consultar pedidos completos e atualizar o status
    /// de pedidos, aplicando regras de negócio específicas da aplicação.
    /// </remarks>
    public interface IPedidoService
    {
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
    }
}
