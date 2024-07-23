using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;

        public RegionsController(NZWalksDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            // Get Data From Database - Domain Model
            var regionsDomain = _context.Regions.ToList();

            // Map Domain Models To DTOs
            var regionsDto = new List<RegionDto>();

            foreach (var region in regionsDomain)
            {
                regionsDto.Add(new RegionDto() { 
                    Id = region.Id, 
                    Name = region.Name, 
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                }); 
            }

            // Return DTOs
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute] Guid id) 
        {
            // var region = _context.Regions.Find(id);
            // Getting the Region Domain model from the database
            var regionDomainModel = _context.Regions.FirstOrDefault(x => x.Id == id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Mapping Region Domain model to DTO model
            var regionDto = new RegionDto() {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(regionDto);
        }

        [HttpPost]
        public IActionResult Create(AddRegionRequestDto requestDto)
        {
            var regionDomainModel = new Region()
            {
                Code = requestDto.Code,
                RegionImageUrl = requestDto.RegionImageUrl,
                Name = requestDto.Name,
            };

            _context.Regions.Add(regionDomainModel);
            _context.SaveChanges();

            var adddedRegionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, adddedRegionDto);
        }

        //Update Method Data Annotations
        // We will send the id of the region to be updated
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto regionRequestDto) 
        {
            // Check if the object does exist or not
            var regionDomainModel = _context.Regions.FirstOrDefault(x => x.Id == id);
             
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Map Dto we got from client to the domain model
            regionDomainModel.Name = regionRequestDto.Name;
            regionDomainModel.RegionImageUrl = regionRequestDto.RegionImageUrl;
            regionDomainModel.Code = regionRequestDto.Code;
            _context.SaveChanges();

            // Creating a RegionDto to return it to the client
            var regionDto = new RegionDto()
            {
                Name = regionDomainModel.Name,
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionRequestDto.RegionImageUrl,
           
            };

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = _context.Regions.FirstOrDefault(x => x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(regionDomainModel);
            _context.SaveChanges();

            var deletedRegionDtoModel = new RegionDto()
            {
                Name= regionDomainModel.Name,
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(deletedRegionDtoModel);
        }
    }
}
