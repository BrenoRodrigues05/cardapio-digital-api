using AutoMapper;
using cardapio_digital_api.DTOs;
using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace cardapio_digital_api.Controllers
{
    /// <summary>
    /// Controller responsável pelo gerenciamento de produtos.
    /// </summary>
    /// <remarks>
    /// Fornece endpoints para CRUD genérico de produtos, além de operações específicas
    /// como listar produtos por restaurante, atualizar disponibilidade e aplicar filtros.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : GenericControllerDTO<Produto, ProdutoCreateDTO, ProdutoReadDTO>
    {
        public ProdutosController(IRepository<Produto> repository, IMapper mapper,
            ILogger<ProdutosController> logger, IUnitOfWork unitOfWork)
            : base(repository, mapper, logger, unitOfWork)
        {
        }

        /// <summary>
        /// Lista todos os produtos de um restaurante específico.
        /// </summary>
        /// <param name="restaurantId">ID do restaurante.</param>
        /// <returns>Lista de produtos pertencentes ao restaurante.</returns>
        /// <response code="200">Retorna a lista de produtos.</response>
        /// <response code="404">Nenhum produto encontrado para este restaurante.</response>
        [HttpGet("restaurante/{restaurantId}")]
        public async Task<ActionResult<IEnumerable<ProdutoReadDTO>>> GetProductsFromRestaurant(int restaurantId)
        {
            var products = await _repository.GetAllAsync(); // pega todos
            var filteredProducts = products.Where(p => p.RestauranteId == restaurantId);

            if (products == null)
                return NotFound("Nenhum produto encontrado para este restaurante.");

            return Ok(_mapper.Map<IEnumerable<ProdutoReadDTO>>(filteredProducts));
        }

        /// <summary>
        /// Deleta um produto de um restaurante específico.
        /// </summary>
        /// <param name="restaurantId">ID do restaurante.</param>
        /// <param name="productId">ID do produto a ser deletado.</param>
        /// <returns>NoContent se deletado com sucesso.</returns>
        /// <response code="204">Produto deletado com sucesso.</response>
        /// <response code="404">Produto não encontrado para o restaurante informado.</response>
        [HttpDelete("restaurante/{restaurantId}/produto/{productId}")]
        public async Task<IActionResult> DeleteProductFromRestaurant(int restaurantId, int productId)
        {
            var product = await _repository.GetByPredicateAsync(p =>
                p.Id == productId && p.RestauranteId == restaurantId);

            if (product == null)
                return NotFound("Produto não encontrado para este restaurante.");

            _repository.Remove(product);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        /// <summary>
        /// Atualiza a disponibilidade de um produto.
        /// </summary>
        /// <param name="productId">ID do produto.</param>
        /// <param name="available">Novo status de disponibilidade do produto.</param>
        /// <returns>NoContent se atualizado com sucesso.</returns>
        /// <response code="204">Disponibilidade atualizada com sucesso.</response>
        /// <response code="404">Produto não encontrado.</response>
        [HttpPatch("{productId}/disponivel")]
        public async Task<IActionResult> UpdateAvailability(int productId, [FromQuery] bool available)
        {
            var product = await _repository.GetByIdAsync(productId);
            if (product == null)
                return NotFound("Produto não encontrado.");

            product.Disponivel = available;
            _repository.Update(product);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        /// <summary>
        /// Lista produtos filtrando por restaurante, preço máximo e disponibilidade.
        /// </summary>
        /// <param name="restaurantId">ID do restaurante (opcional).</param>
        /// <param name="MaxValue">Preço máximo do produto (opcional).</param>
        /// <returns>Lista de produtos filtrados.</returns>
        /// <response code="200">Retorna a lista de produtos filtrados.</response>
        /// <response code="404">Nenhum produto encontrado com os filtros aplicados.</response>
        /// <remarks>
        /// Exemplos de uso:
        /// GET /api/produtos/filtro?restauranteId=1EprecoMax=50
        /// GET /api/produtos/filtro?precoMax=30
        /// GET /api/produtos/filtro?restauranteId=2
        /// </remarks>
        [HttpGet("filtro")]
        public async Task<ActionResult<IEnumerable<ProdutoReadDTO>>> FiltroProdutos(
            [FromQuery] int? restaurantId,
            [FromQuery] decimal? MaxValue)
        {
            var products = await _repository.GetAllAsync();
            var filteredProducts = products.Where(p =>
                (!restaurantId.HasValue || p.RestauranteId == restaurantId.Value) &&
                (!MaxValue.HasValue || p.Preco <= MaxValue.Value) &&
                p.Disponivel);

            if (!filteredProducts.Any())
                return NotFound("Nenhum produto encontrado com os filtros aplicados.");

            return Ok(_mapper.Map<IEnumerable<ProdutoReadDTO>>(filteredProducts));
        }
    }
}
