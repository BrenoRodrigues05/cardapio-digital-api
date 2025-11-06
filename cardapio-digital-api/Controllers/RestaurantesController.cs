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
    public class RestaurantesController : GenericControllerDTO<Restaurante,
        RestauranteCreateDTO, RestauranteReadDTO>
    {
        public RestaurantesController(IRepository<Restaurante> repository, IMapper mapper, 
            ILogger<RestaurantesController> logger, IUnitOfWork unitOfWork) : base(repository, mapper, logger, unitOfWork)
        {
        }
    }
}
