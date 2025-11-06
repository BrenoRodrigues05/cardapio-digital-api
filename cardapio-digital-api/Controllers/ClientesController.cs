using AutoMapper;
using cardapio_digital_api.DTOs;
using cardapio_digital_api.Models;
using cardapio_digital_api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cardapio_digital_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : GenericControllerDTO<Cliente, ClienteCreateDTO, ClienteReadDTO>
    {
        public ClientesController(IRepository<Cliente> repository, IMapper mapper,
            ILogger<ClientesController> logger, IUnitOfWork unitOfWork)
            : base(repository, mapper, logger, unitOfWork)
        {
        }
    }
}
