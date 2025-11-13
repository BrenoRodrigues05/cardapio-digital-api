using AutoMapper;
using cardapio_digital_api.DTOs;
using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;
using cardapio_digital_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly IPedidoService _pedidoService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="PedidosController"/>.
        /// </summary>
        /// <param name="logger">Logger para registrar informações e erros.</param>
        /// <param name="pedidoService">Serviço responsável pelas operações de negócio relacionadas a pedidos.</param>
        /// <param name="mapper">AutoMapper para conversão entre entidades e DTOs.</param>
        public PedidosController(
            ILogger<PedidosController> logger,
            IPedidoService pedidoService,
            IMapper mapper)
        {
            _logger = logger;
            _pedidoService = pedidoService;
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
            var orders = await _pedidoService.GetAllPedidosAsync();
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
            var allOrders = await _pedidoService.GetPedidosPorClienteAsync(clienteId);

            if(!allOrders.Any()) return NotFound("Nenhum pedido encontrado para este cliente.");

            var ordersDto = _mapper.Map<IEnumerable<PedidoReadDTO>>(allOrders);
           
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
            var order = await _pedidoService.GetPedidoCompletoAsync(id);

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

            var order = _mapper.Map<Pedido>(pedidoDto);

            try
            {
                var pedidoId = await _pedidoService.CriarPedidoAsync(order);
                var pedidoReadDto = _mapper.Map<PedidoReadDTO>(await _pedidoService.
                    GetPedidoCompletoAsync(pedidoId));
                return CreatedAtAction(nameof(GetById), new { id = pedidoId }, pedidoReadDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            var updated = await _pedidoService.AtualizarStatusPedidoAsync(id, status);
            if (!updated) return NotFound("Pedido não encontrado.");
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
            var deleted = await _pedidoService.DeletarPedidoAsync(id);
            if (!deleted) return NotFound("Pedido não encontrado.");
            return NoContent();
        }
    }
}
