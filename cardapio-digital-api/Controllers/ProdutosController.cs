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
    public class ProdutosController : GenericControllerDTO<Produto, ProdutoCreateDTO, ProdutoReadDTO>
    {
        public ProdutosController(IRepository<Produto> repository, IMapper mapper, 
            ILogger<ProdutosController> logger, IUnitOfWork unitOfWork) : base(repository, mapper, logger, unitOfWork)
        {
        }

    }
}
