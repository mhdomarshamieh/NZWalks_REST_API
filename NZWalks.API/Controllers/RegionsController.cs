using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbContext context, IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            // Get Data From Database - Domain Model
            var regionsDomain = await _regionRepository.GetAllAsync();

            //var regionsDto = new List<RegionDto>()

            // Map Domain Models To DTOs
            var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);

            // Return DTOs
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id) 
        {
            // var region = _context.Regions.Find(id);
            // Getting the Region Domain model from the database
            var regionDomainModel = await _regionRepository.GetByIdAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Mapping Region Domain model to DTO model
            
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create(AddRegionRequestDto requestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var regionDomainModel = _mapper.Map<Region>(requestDto);

            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

            var adddedRegionDto = _mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, adddedRegionDto);
        }

        //Update Method Data Annotations
        // We will send the id of the region to be updated
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto regionRequestDto) 
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Map the DTO to a Domain model
            var regionDomainModel = _mapper.Map<Region>(regionRequestDto);

            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);
             
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Creating a RegionDto to return it to the client
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            if( regionDomainModel == null)
            {
                return NotFound();
            }

            var deletedRegionDtoModel = _mapper.Map<RegionDto>(regionDomainModel);

            return Ok(deletedRegionDtoModel);
        }
    }
}
