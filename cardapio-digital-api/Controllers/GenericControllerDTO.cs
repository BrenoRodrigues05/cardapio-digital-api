using AutoMapper;
using cardapio_digital_api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cardapio_digital_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericControllerDTO<TEntity, TCreateDTO, TReadDTO> : ControllerBase
        where TEntity : class
        where TCreateDTO : class
        where TReadDTO : class
     
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger <GenericControllerDTO<TEntity, TCreateDTO, TReadDTO>> _logger;
        protected readonly IUnitOfWork _unitOfWork;

        public GenericControllerDTO(IRepository<TEntity> repository, IMapper mapper,
            ILogger<GenericControllerDTO<TEntity, TCreateDTO, TReadDTO>> logger, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<TReadDTO>>> GetAll()
        {
            _logger.LogInformation("Fetching all entities of type {EntityType}", typeof(TEntity).Name);

            var entities = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<TReadDTO>>(entities);

            _logger.LogInformation("Successfully fetched all entities of type {EntityType}",
                typeof(TEntity).Name);
            return Ok(dtos);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<TReadDTO>> GetById(int id)
        {
            _logger.LogInformation("Fetching entity of type {EntityType} with ID {Id}",
                typeof(TEntity).Name, id);
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Entity of type {EntityType} with ID {Id} not found",
                    typeof(TEntity).Name, id);
                return NotFound();
            }
            var dto = _mapper.Map<TReadDTO>(entity);
            _logger.LogInformation("Successfully fetched entity of type {EntityType} with ID {Id}",
                typeof(TEntity).Name, id);
            return Ok(dto);
        }

        [HttpPost]

        public async Task<ActionResult<TReadDTO>> Create([FromBody]TCreateDTO createDto)
        {
            _logger.LogInformation("Creating new entity of type {EntityType}", typeof(TEntity).Name);
            var entity = _mapper.Map<TEntity>(createDto);
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            var readDto = _mapper.Map<TReadDTO>(entity);
            _logger.LogInformation("Successfully created entity of type {EntityType}", typeof(TEntity).Name);
            return Ok(_mapper.Map<TReadDTO>(entity));
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody]TCreateDTO updateDto)
        {
            _logger.LogInformation("Updating entity of type {EntityType} with ID {Id}",
                typeof(TEntity).Name, id);
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                _logger.LogWarning("Entity of type {EntityType} with ID {Id} not found",
                    typeof(TEntity).Name, id);
                return NotFound();
            }
            _mapper.Map(updateDto, existingEntity);
            _repository.Update(existingEntity);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Successfully updated entity of type {EntityType} with ID {Id}",
                typeof(TEntity).Name, id);
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> RemoveAsync(int id)
        {
            _logger.LogInformation("Deleting entity of type {EntityType} with ID {Id}",
                typeof(TEntity).Name, id);
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                _logger.LogWarning("Entity of type {EntityType} with ID {Id} not found",
                    typeof(TEntity).Name, id);
                return NotFound();
            }
            _repository.Remove(existingEntity);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Successfully deleted entity of type {EntityType} with ID {Id}",
                typeof(TEntity).Name, id);
            return NoContent();
        }
    }
}
