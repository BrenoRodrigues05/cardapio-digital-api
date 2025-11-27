using cardapio_digital_api.Context;
using cardapio_digital_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital_api.Repositories
{
    public class ItemPedidoRepository : Repository<ItemPedido>, IItemPedidoRepository
    {
        public ItemPedidoRepository(CardapioDigitalDbContext ctx) : base(ctx)
        {
        }

        public async Task AdicionarOuIncrementarAsync(int pedidoId, int produtoId, int quantidade, decimal precoUnitario)
        {
            var itemExistente = await _dbSet
                .FirstOrDefaultAsync(ip => ip.PedidoId == pedidoId && ip.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                // Incrementa quantidade
                itemExistente.Quantidade += quantidade;

                // Manter o preço sempre atualizado:
                itemExistente.PrecoUnitario = precoUnitario;

                _dbSet.Update(itemExistente);
                return;
            }

            // Se não existe, cria novo
            var novoItem = new ItemPedido
            {
                PedidoId = pedidoId,
                ProdutoId = produtoId,
                Quantidade = quantidade,
                PrecoUnitario = precoUnitario
            };

            await _dbSet.AddAsync(novoItem);

        }
        public async Task<ItemPedido?> ObterItemAsync(int pedidoId, int produtoId)
        {
            return await _dbSet.FirstOrDefaultAsync(ip =>
                ip.PedidoId == pedidoId &&
                ip.ProdutoId == produtoId);
        }
    }
}
