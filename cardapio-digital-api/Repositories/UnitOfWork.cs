using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Implementa o padrão <see cref="IUnitOfWork"/>, responsável por coordenar 
    /// e gerenciar os repositórios da aplicação, garantindo a integridade das 
    /// transações e a persistência consistente dos dados no contexto do banco.
    /// </summary>
    /// <remarks>
    /// Essa classe atua como um ponto central para o gerenciamento das operações 
    /// de repositório, permitindo que múltiplas alterações sejam realizadas e 
    /// confirmadas de forma atômica por meio do método <see cref="CommitAsync"/>.
    /// </remarks>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Contexto de banco de dados utilizado para persistência das entidades.
        /// </summary>
        private readonly CardapioDigitalDbContext _context;

        /// <summary>
        /// Repositório responsável pelas operações de persistência e consulta dos clientes.
        /// </summary>
        public IRepository<Cliente> Clientes { get; }

        /// <summary>
        /// Repositório responsável pelas operações de persistência e consulta dos itens de pedido.
        /// </summary>
        public IRepository<ItemPedido> ItensPedido { get; }

        /// <summary>
        /// Repositório responsável pelas operações de persistência e consulta dos pedidos.
        /// </summary>
        public IPedidoRepository Pedidos { get; }

        /// <summary>
        /// Repositório responsável pelas operações de persistência e consulta dos produtos.
        /// </summary>
        public IRepository<Produto> Produtos { get; }

        /// <summary>
        /// Repositório responsável pelas operações de persistência e consulta dos restaurantes.
        /// </summary>
        public IRepository<Restaurante> Restaurantes { get; }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="UnitOfWork"/> com os repositórios e o contexto especificados.
        /// </summary>
        /// <param name="context">O contexto do banco de dados utilizado para gerenciar as entidades.</param>
        /// <param name="clientes">Instância do repositório de clientes.</param>
        /// <param name="itensPedido">Instância do repositório de itens de pedido.</param>
        /// <param name="pedidos">Instância do repositório de pedidos.</param>
        /// <param name="produtos">Instância do repositório de produtos.</param>
        /// <param name="restaurantes">Instância do repositório de restaurantes.</param>
        /// <example>
        /// Exemplo de uso:
        /// <code>
        /// using (var uow = new UnitOfWork(context, repoClientes, repoItens, repoPedidos, repoProdutos, repoRestaurantes))
        /// {
        ///     var novoCliente = new Cliente { Nome = "Maria Oliveira", Email = "maria@exemplo.com" };
        ///     uow.Clientes.Add(novoCliente);
        ///     await uow.CommitAsync();
        /// }
        /// </code>
        /// </example>
        public UnitOfWork(
            CardapioDigitalDbContext context,
            IRepository<Cliente> clientes,
            IRepository<ItemPedido> itensPedido,
            IPedidoRepository pedidos,
            IRepository<Produto> produtos,
            IRepository<Restaurante> restaurantes)
        {
            _context = context;
            Clientes = clientes;
            ItensPedido = itensPedido;
            Pedidos = pedidos;
            Produtos = produtos;
            Restaurantes = restaurantes;
        }

        /// <summary>
        /// Confirma (commita) todas as alterações realizadas nos repositórios dentro do contexto atual.
        /// </summary>
        /// <returns>
        /// Um <see cref="Task{Int32}"/> representando a operação assíncrona.
        /// O valor retornado indica o número de registros afetados no banco de dados.
        /// </returns>
        /// <exception cref="DbUpdateException">
        /// Lançada quando ocorre um erro ao salvar as alterações no banco de dados.
        /// </exception>
        public Task<int> CommitAsync() => _context.SaveChangesAsync();

        /// <summary>
        /// Libera os recursos não gerenciados utilizados pelo contexto de banco de dados.
        /// </summary>
        /// <remarks>
        /// Esse método deve ser chamado quando a unidade de trabalho não for mais necessária,
        /// a fim de liberar conexões e memória associadas ao contexto do Entity Framework.
        /// </remarks>
        public void Dispose() => _context.Dispose();
    }
}
