using AutoMapper;
using cardapio_digital_api.DTOs;
using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace cardapio_digital_api.Controllers
{
    /// <summary>
    /// Controller responsável pelo gerenciamento de pedidos no estilo iFood.
    /// </summary>
    /// <remarks>
    /// Este controller fornece endpoints para:
    /// - Listar todos os pedidos
    /// - Listar pedidos de um cliente específico
    /// - Obter detalhes de um pedido pelo ID
    /// - Criar um novo pedido
    /// - Atualizar o status de um pedido
    /// - Deletar um pedido
    /// 
    /// As respostas utilizam DTOs específicos:
    /// <see cref="PedidoReadDTO"/> para consultas e <see cref="PedidoCreateDTO"/> para criação.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly ILogger<PedidosController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Pedido> _pedidoRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Produto> _produtoRepository;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="PedidosController"/>.
        /// </summary>
        /// <param name="logger">Logger para registrar informações e erros.</param>
        /// <param name="unitOfWork">Unit of Work para persistência de dados.</param>
        /// <param name="pedidoRepository">Repositório de pedidos para operações CRUD.</param>
        /// <param name="produtoRepository">Repositório de produtos para buscar e atualizar estoque.</param>
        /// <param name="mapper">AutoMapper para conversão entre entidades e DTOs.</param>
        public PedidosController(
            ILogger<PedidosController> logger,
            IUnitOfWork unitOfWork,
            IRepository<Pedido> pedidoRepository,
            IRepository<Produto> produtoRepository,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todos os pedidos cadastrados.
        /// </summary>
        /// <returns>Lista de pedidos com informações completas.</returns>
        /// <response code="200">Pedidos retornados com sucesso.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoReadDTO>>> GetAll()
        {
            _logger.LogInformation("Fetching all orders");
            var orders = await _pedidoRepository.GetAllAsync();
            var ordersDto = _mapper.Map<IEnumerable<PedidoReadDTO>>(orders);
            _logger.LogInformation("Fetched {Count} orders", ordersDto.Count());
            return Ok(ordersDto);
        }

        /// <summary>
        /// Lista todos os pedidos de um cliente específico.
        /// </summary>
        /// <param name="clienteId">ID do cliente.</param>
        /// <returns>Lista de pedidos do cliente informado.</returns>
        /// <response code="200">Pedidos do cliente retornados com sucesso.</response>
        /// <response code="404">Nenhum pedido encontrado para o cliente informado.</response>
        /// <example>GET /api/pedidos/cliente/3</example>
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<PedidoReadDTO>>> GetByClientId(int clienteId)
        {
            _logger.LogInformation("Fetching orders for client ID: {ClienteId}", clienteId);

            // Pega todos os pedidos do banco
            var allOrders = await _pedidoRepository.GetAllAsync();

            // Filtra pelo cliente
            var orders = allOrders.Where(p => p.ClienteId == clienteId).ToList();

            if (!orders.Any())
            {
                _logger.LogWarning("No orders found for client ID: {ClienteId}", clienteId);
                return NotFound("Nenhum pedido encontrado para este cliente.");
            }

            var ordersDto = _mapper.Map<IEnumerable<PedidoReadDTO>>(orders);
            _logger.LogInformation("Successfully fetched {Count} orders for client ID: {ClienteId}", 
                orders.Count, clienteId);

            return Ok(ordersDto);
        }

        /// <summary>
        /// Obtém detalhes de um pedido pelo seu ID.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        /// <returns>Informações detalhadas do pedido.</returns>
        /// <response code="200">Pedido encontrado e retornado com sucesso.</response>
        /// <response code="404">Pedido não encontrado para o ID informado.</response>
        /// <example>GET /api/pedidos/5</example>
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoReadDTO>> GetById(int id)
        {
            _logger.LogInformation("Fetching order with ID: {Id}", id);
            var order = await _pedidoRepository.GetByIdAsync(id);

            if (order == null)
            {
                _logger.LogWarning("Order with ID: {Id} not found", id);
                return NotFound("Pedido não encontrado.");
            }

            var orderDto = _mapper.Map<PedidoReadDTO>(order);
            return Ok(orderDto);
        }

        /// <summary>
        /// Cria um novo pedido para um cliente em um restaurante específico.
        /// </summary>
        /// <remarks>
        /// O cliente deve informar:
        /// - <c>clienteId</c>
        /// - <c>restauranteId</c>
        /// - Lista de itens contendo apenas <c>produtoId</c> e <c>quantidade</c>
        ///
        /// O sistema irá automaticamente:
        /// 1. Buscar os produtos no banco de dados.
        /// 2. Verificar se o produto está disponível e possui estoque suficiente.
        /// 3. Calcular o subtotal de cada item (quantidade × preço unitário).
        /// 4. Atualizar o estoque do produto (diminuindo a quantidade vendida e ajustando o status de disponibilidade).
        /// 5. Calcular o valor total do pedido.
        /// 
        /// Se algum produto estiver indisponível ou sem estoque suficiente, será retornada uma mensagem de erro.
        /// </remarks>
        /// <param name="pedidoDto">Objeto <see cref="PedidoCreateDTO"/> contendo as informações do pedido.</param>
        /// <returns>Um <see cref="PedidoReadDTO"/> com todos os detalhes do pedido criado, incluindo itens e valor total.</returns>
        /// <response code="201">Pedido criado com sucesso. Retorna o objeto <see cref="PedidoReadDTO"/>.</response>
        /// <response code="400">Falha na validação do pedido. Possíveis motivos:
        /// - Nenhum item informado
        /// - Produto indisponível
        /// - Quantidade solicitada maior que o estoque disponível
        /// </response>
        /// <response code="404">Produto informado no pedido não foi encontrado no sistema.</response>
        /// <example>
        /// Requisição POST /api/pedidos
        /// {
        ///   "clienteId": 3,
        ///   "restauranteId": 2,
        ///   "itens": [
        ///     { "produtoId": 5, "quantidade": 2 },
        ///     { "produtoId": 7, "quantidade": 1 }
        ///   ]
        /// }
        /// </example>
        /// <example>
        /// Resposta 201:
        /// {
        ///   "id": 12,
        ///   "dataPedido": "2025-11-07T15:30:00Z",
        ///   "status": "Aberto",
        ///   "clienteId": 3,
        ///   "cliente": {
        ///     "id": 3,
        ///     "nome": "João Silva",
        ///     "email": "joao@email.com",
        ///     "telefone": "11999999999"
        ///   },
        ///   "restauranteId": 2,
        ///   "restaurante": {
        ///     "id": 2,
        ///     "nome": "Restaurante Exemplo",
        ///     "categoria": "Italiana",
        ///     "telefone": "1122223333"
        ///   },
        ///   "itens": [
        ///     { "produtoId": 5, "quantidade": 2, "precoUnitario": 19.90, "subtotal": 39.80 },
        ///     { "produtoId": 7, "quantidade": 1, "precoUnitario": 9.50, "subtotal": 9.50 }
        ///   ],
        ///   "valorTotal": 49.30
        /// }
        /// </example>
        [HttpPost]
        public async Task<ActionResult<PedidoReadDTO>> Create([FromBody] PedidoCreateDTO pedidoDto)
        {
            _logger.LogInformation("Creating a new order for client ID: {ClienteId}", pedidoDto.ClienteId);

            if (pedidoDto.Itens == null || !pedidoDto.Itens.Any())
                return BadRequest("É necessário incluir ao menos um item no pedido.");

            var pedido = new Pedido
            {
                ClienteId = pedidoDto.ClienteId,
                RestauranteId = pedidoDto.RestauranteId,
                Status = "Aberto",
                DataPedido = DateTime.UtcNow,
                Itens = new List<ItemPedido>()
            };

            foreach (var itemDto in pedidoDto.Itens)
            {
                // Buscar o produto
                var produto = await _produtoRepository.GetByIdAsync(itemDto.ProdutoId);
                if (produto == null)
                    return NotFound($"Produto com ID {itemDto.ProdutoId} não encontrado.");

                // Verificar estoque
                if (!produto.Disponivel || produto.QuantidadeEstoque < itemDto.Quantidade)
                    return BadRequest($"Produto '{produto.Nome}' não possui estoque suficiente.");

                // Criar ItemPedido
                var itemPedido = new ItemPedido
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.Preco
                };

                // Diminuir estoque
                produto.QuantidadeEstoque -= itemDto.Quantidade;
                if (produto.QuantidadeEstoque == 0)
                    produto.Disponivel = false;

                _produtoRepository.Update(produto);

                pedido.Itens.Add(itemPedido);
            }

            await _pedidoRepository.AddAsync(pedido);
            await _unitOfWork.CommitAsync();

            var pedidoReadDto = _mapper.Map<PedidoReadDTO>(pedido);
            _logger.LogInformation("Order created with ID: {Id}", pedido.Id);

            return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedidoReadDto);
        }

        /// <summary>
        /// Atualiza o status de um pedido existente.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        /// <param name="status">Novo status do pedido (Aberto, Em Preparo, Entregue, Cancelado).</param>
        /// <returns>NoContent se atualizado com sucesso.</returns>
        /// <response code="204">Status atualizado com sucesso.</response>
        /// <response code="404">Pedido não encontrado.</response>
        /// <example>
        /// PATCH /api/pedidos/5/status?status=Entregue
        /// </example>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            _logger.LogInformation("Updating status for order ID: {Id} to {Status}", id, status);
            var order = await _pedidoRepository.GetByIdAsync(id);

            if (order == null)
            {
                _logger.LogWarning("Order with ID: {Id} not found", id);
                return NotFound("Pedido não encontrado.");
            }

            order.Status = status;
            _pedidoRepository.Update(order);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Status updated for order ID: {Id}", id);
            return NoContent();
        }

        /// <summary>
        /// Deleta um pedido pelo ID.
        /// </summary>
        /// <param name="id">ID do pedido a ser deletado.</param>
        /// <returns>NoContent se deletado com sucesso.</returns>
        /// <response code="204">Pedido deletado com sucesso.</response>
        /// <response code="404">Pedido não encontrado.</response>
        /// <example>DELETE /api/pedidos/5</example>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting order with ID: {Id}", id);
            var order = await _pedidoRepository.GetByIdAsync(id);

            if (order == null)
            {
                _logger.LogWarning("Order with ID: {Id} not found", id);
                return NotFound("Pedido não encontrado.");
            }

            _pedidoRepository.Remove(order);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Order deleted with ID: {Id}", id);
            return NoContent();
        }
    }
}
