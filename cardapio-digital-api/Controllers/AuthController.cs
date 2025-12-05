using cardapio_digital_api.DTOs;
using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;
using cardapio_digital_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Claims;

namespace cardapio_digital_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;

        public AuthController(IUsuarioService usuarioService, ITokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] UsuarioCreateDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sucesso = await _usuarioService.RegisterAsync(dto);

            if(!sucesso)
            {
                return BadRequest(new { message = "Email ou CPF/CNPJ já existe." });
            }

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _usuarioService.AuthenticateAsync(loginModel.Email!, loginModel.Password!);

            if(usuario == null)
            {
                return Unauthorized(new { message = "Credenciais inválidas." });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Name),
                new Claim(ClaimTypes.Email, usuario.Email),
            };

            var token = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Ok(new
            {
                token,
                refreshToken,
                usuario = new
                {
                    usuario.Id,
                    usuario.Name,
                    usuario.Email
                }
            });
        }
        
        


    }
}
