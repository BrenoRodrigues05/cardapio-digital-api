using cardapio_digital_api.Models;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Define o contrato para a implementação do padrão Unit of Work (Unidade de Trabalho),
    /// que coordena o acesso a múltiplos repositórios e garante que todas as operações
    /// sejam concluídas dentro de uma única transação lógica.
    /// </summary>
    /// <remarks>
    /// Essa interface é responsável por gerenciar os repositórios e consolidar as alterações
    /// realizadas no contexto de dados, permitindo confirmar (commit) ou descartar (rollback)
    /// as transações de forma controlada.
    /// </remarks>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repositório responsável pelas operações de persistência e consulta relacionadas aos clientes.
        /// </summary>
        IRepository<Cliente> Clientes { get; }

        /// <summary>
        /// Repositório responsável pelas operações de persistência e consulta dos itens dos pedidos.
        /// </summary>
        IRepository<ItemPedido> ItensPedido { get; }

        /// <summary>
        /// Repositório responsável pelas operações de persistência e consulta de pedidos realizados.
        /// </summary>
        IPedidoRepository Pedidos { get; }

        /// <summary>
        /// Repositório responsável pelas operações de persistência e consulta dos produtos do cardápio.
        /// </summary>
        IRepository<Produto> Produtos { get; }

        /// <summary>
        /// Repositório responsável pelas operações de persistência e consulta dos restaurantes cadastrados.
        /// </summary>
        IRepository<Restaurante> Restaurantes { get; }

        /// <summary>
        /// Persiste todas as alterações realizadas nos repositórios associados à unidade de trabalho.
        /// </summary>
        /// <returns>
        /// Retorna um <see cref="Task{Int32}"/> representando a operação assíncrona,
        /// cujo valor indica o número de registros afetados no banco de dados.
        /// </returns>
        /// <example>
        /// Exemplo de uso:
        /// <code>
        /// using (var uow = new UnitOfWork(context))
        /// {
        ///     var novoProduto = new Produto { Nome = "Pizza", Preco = 29.90M };
        ///     uow.Produtos.Add(novoProduto);
        ///     await uow.CommitAsync();
        /// }
        /// </code>
        /// </example>
        Task<int> CommitAsync();
    }
}
