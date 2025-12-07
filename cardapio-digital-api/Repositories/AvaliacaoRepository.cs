using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    /// <summary>
    /// Repositório responsável por operações de acesso a dados da entidade <see cref="Avaliacao"/>.
    /// Implementa consultas customizadas relacionadas a clientes, entregadores e restaurantes.
    /// </summary>
    public class AvaliacaoRepository : Repository<Avaliacao>, IAvaliacaoRepository
    {
        /// <summary>
        /// Inicializa uma nova instância de <see cref="AvaliacaoRepository"/>.
        /// </summary>
        /// <param name="ctx">Contexto do banco de dados.</param>
        public AvaliacaoRepository(CardapioDigitalDbContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Retorna todas as avaliações feitas por um cliente específico.
        /// </summary>
        /// <param name="clienteId">ID do cliente.</param>
        /// <returns>Lista de avaliações realizadas pelo cliente informado.</returns>
        public async Task<IEnumerable<Avaliacao>> GetByClienteAsync(int clienteId)
        {
            return await _ctx.Avaliacoes
                 .Where(a => a.ClienteId == clienteId)
                 .Include(a => a.Cliente)
                 .Include(a => a.Restaurante)
                 .Include(a => a.Entregador)
                 .OrderByDescending(a => a.Data)
                 .ToListAsync();
        }

        /// <summary>
        /// Retorna todas as avaliações destinadas a um entregador específico.
        /// </summary>
        /// <param name="entregadorId">ID do entregador.</param>
        /// <returns>Lista de avaliações relacionadas ao entregador informado.</returns>
        public async Task<IEnumerable<Avaliacao>> GetByEntregadorAsync(int entregadorId)
        {
            return await _ctx.Avaliacoes
                .Where(a => a.EntregadorId == entregadorId)
                .Include(a => a.Cliente)
                .Include(a => a.Entregador)
                .OrderByDescending(a => a.Data)
                .ToListAsync();
        }

        /// <summary>
        /// Retorna todas as avaliações feitas para um restaurante específico.
        /// </summary>
        /// <param name="restauranteId">ID do restaurante.</param>
        /// <returns>Lista de avaliações associadas ao restaurante informado.</returns>
        public async Task<IEnumerable<Avaliacao>> GetByRestauranteAsync(int restauranteId)
        {
            return await _ctx.Avaliacoes
                .Where(a => a.RestauranteId == restauranteId)
                .Include(a => a.Cliente)
                .Include(a => a.Restaurante)
                .OrderByDescending(a => a.Data)
                .ToListAsync();
        }

        /// <summary>
        /// Calcula a média das notas recebidas por um entregador específico.
        /// </summary>
        /// <param name="entregadorId">ID do entregador.</param>
        /// <returns>
        /// A média das notas recebidas, ou 0 caso o entregador ainda não tenha sido avaliado.
        /// </returns>
        public async Task<double> GetMediaNotasEntregadorAsync(int entregadorId)
        {
            var avaliacoes = await _ctx.Avaliacoes
                .Where(a => a.EntregadorId == entregadorId)
                .ToListAsync();

            if (!avaliacoes.Any())
                return 0;

            return avaliacoes.Average(a => (double)a.Nota);
        }

        /// <summary>
        /// Calcula a média das notas recebidas por um restaurante específico.
        /// </summary>
        /// <param name="restauranteId">ID do restaurante.</param>
        /// <returns>
        /// A média das notas recebidas, ou 0 caso o restaurante ainda não tenha sido avaliado.
        /// </returns>
        public async Task<double> GetMediaNotasRestauranteAsync(int restauranteId)
        {
            var avaliacoes = await _ctx.Avaliacoes
                .Where(a => a.RestauranteId == restauranteId)
                .ToListAsync();

            if (!avaliacoes.Any())
                return 0;

            return avaliacoes.Average(a => (double)a.Nota);
        }
    }
}
