using cardapio_digital_api.DTOs;
using cardapio_digital_api.Models;

namespace cardapio_digital_api.Services
{
    public interface IUsuarioService
    {
        Task<Usuario?> GetProfileAsync(int usuarioId);
        Task<Usuario?> AuthenticateAsync(string email, string password);
        Task<bool> RegisterAsync(UsuarioCreateDTO dto);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> CpfCnpjExistsAsync(string cpfCnpj);

    }
}
