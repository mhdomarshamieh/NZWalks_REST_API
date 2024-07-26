using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _repository;

        public WalksController(IMapper mapper, IWalkRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
            walkDomainModel = await _repository.CreateAsync(walkDomainModel);

            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
                                                [FromQuery] string? sortBy, [FromQuery] bool isAscending,
                                                [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walkDomainModels = await _repository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            //Map model to dto
            return Ok(_mapper.Map<List<WalkDto>>(walkDomainModels));
        }

        // Walk by Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await _repository.GetByIdAsync(id);

            if(walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        //Update walk by Id
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Map Dto to domain model
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

            walkDomainModel = await _repository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();

            }

            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        //Delete Walk By Id
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await _repository.DeleteAsync(id);

            if(walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
