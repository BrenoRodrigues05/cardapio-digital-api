using cardapio_digital_api.DTOs;
using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;
using Microsoft.AspNetCore.Identity;

namespace cardapio_digital_api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsuarioService> _logger;
        private readonly PasswordHasher<Usuario> _passwordHasher = new PasswordHasher<Usuario>();

        public UsuarioService(IUnitOfWork unitOfWork, ILogger<UsuarioService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Usuario?> GetProfileAsync(int usuarioId)
        {
            if(usuarioId <= 0)
            {
                _logger.LogWarning("GetProfileAsync called with invalid usuarioId: {UsuarioId}", usuarioId);
                throw new ArgumentException("Invalid usuarioId", nameof(usuarioId));
            }

            var buscaUsuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);

            if(buscaUsuario == null)
            {
                _logger.LogInformation("Usuario with ID {UsuarioId} not found.", usuarioId);
                return null;
            }

            _logger.LogInformation("Usuario with ID {UsuarioId} retrieved successfully.", usuarioId);

            return buscaUsuario;
        }

        public async Task<Usuario?> AuthenticateAsync(string email, string password)
        {
            if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("AuthenticateAsync called with invalid email or password.");
                throw new ArgumentException("Email and password must be provided.");
            }

            var usuario = await _unitOfWork.Usuarios.GetByEmailAsync(email);

            if(usuario == null)
            {
                _logger.LogInformation("Authentication failed for email {Email}: user not found.", email);
                return null;
            }

            var isPasswordValid = VerifyPassword(password, usuario.PasswordHash, usuario);

            if(!isPasswordValid)
            {
                _logger.LogInformation("Authentication failed for email {Email}: invalid password.", email);
                return null;
            }

            _logger.LogInformation("User with email {Email} authenticated successfully.", email);

            return usuario;
        }

        public async Task<bool> CpfCnpjExistsAsync(string cpfCnpj)
        {
            if(string.IsNullOrWhiteSpace(cpfCnpj))
            {
                _logger.LogWarning("CpfCnpjExistsAsync called with invalid cpfCnpj.");
                throw new ArgumentException("cpfCnpj must be provided.", nameof(cpfCnpj));
            }

            var existe = await _unitOfWork.Usuarios.CpfCnpjExistsAsync(cpfCnpj);

            if(!existe)
            {
                _logger.LogInformation("CPF/CNPJ {CpfCnpj} does not exist.", cpfCnpj);
                return false;
            }
           
            _logger.LogInformation("CPF/CNPJ {CpfCnpj} already exists.", cpfCnpj);

            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("EmailExistsAsync called with invalid email.");
                throw new ArgumentException("email must be provided.", nameof(email));
            }

            var existe = await _unitOfWork.Usuarios.EmailExistsAsync(email);

            if (!existe)
            {
                _logger.LogInformation("Email {Email} does not exist.", email);
                return false;
            }

            _logger.LogInformation("Email {Email} already exists.", email);

            return true;
        }

        public async Task<bool> RegisterAsync(UsuarioCreateDTO dto)
        {
           if(dto == null)
           {
                _logger.LogWarning("RegisterAsync called with null usuario.");
                throw new ArgumentNullException(nameof(dto));
           }

              var existe = await EmailExistsAsync(dto.Email);

            if (existe)
            {
                _logger.LogInformation("Registration failed: Email {Email} already exists.", dto.Email);
                return false;
            }

            var cpfCnpjExists = await CpfCnpjExistsAsync(dto.CpfCnpj);

            if (cpfCnpjExists)
            {
                _logger.LogInformation("Registration failed: CPF/CNPJ {CpfCnpj} already exists.", dto.CpfCnpj);
                return false;
            }

            var usuario = new Usuario
            {
                Name = dto.Name,
                Email = dto.Email,
                CpfCnpj = dto.CpfCnpj
            };

            // Hash da senha
            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, dto.Password);

            await _unitOfWork.Usuarios.AddAsync(usuario);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("User with Email {Email} registered successfully.", dto.Email);

            return true;
        }

        // This is a placeholder for password verification logic.
        private bool VerifyPassword(string password, string passwordHash, Usuario usuario)
        {
            var result = _passwordHasher.VerifyHashedPassword(usuario, passwordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
