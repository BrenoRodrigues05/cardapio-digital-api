using cardapio_digital_api.Models;

namespace cardapio_digital_api.Services
{
    public interface IClienteService
    {
        Task<Cliente> CriarAsync(Cliente cliente);
        Task<Cliente?> ObterPorIdAsync(int id);
        Task<IEnumerable<Cliente>> ObterTodosAsync();
        Task<bool> AtualizarAsync(Cliente cliente);
        Task<bool> RemoverAsync(int id);
    }
}
