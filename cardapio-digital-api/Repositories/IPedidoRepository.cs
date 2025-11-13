using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Repositório específico para operações sobre <see cref="Pedido"/>.
    /// </summary>
    /// <remarks>
    /// Esta interface estende <see cref="IRepository{Pedido}"/> e adiciona operações
    /// específicas para a entidade Pedido que não fazem parte do repositório genérico.
    /// </remarks>
    /// <seealso cref="IRepository{Pedido}"/>
    public interface IPedidoRepository : IRepository<Pedido>
    {
        /// <summary>
        /// Obtém um pedido com todos os dados relacionados carregados de forma completa.
        /// </summary>
        /// <param name="id">Identificador único do pedido a ser recuperado.</param>
        /// <returns>
        /// Uma <see cref="Task{TResult}"/> que, ao completar, contém o <see cref="Pedido"/>
        /// com seus relacionamentos carregados (por exemplo: itens do pedido, cliente,
        /// endereço de entrega, status, formas de pagamento, etc.), ou <c>null</c> se nenhum
        /// pedido com o identificador fornecido for encontrado.
        /// </returns>
        /// <remarks>
        /// "Pedido completo" refere-se ao carregamento das coleções e entidades relacionadas
        /// necessárias para exibir ou processar o pedido sem necessidade de chamadas adicionais.
        /// A composição exata dos relacionamentos carregados depende da implementação concreta
        /// do repositório (por exemplo, uso de Include em Entity Framework).
        /// </remarks>
        Task<Pedido?> GetPedidoCompletoAsync(int id);
    }
}
