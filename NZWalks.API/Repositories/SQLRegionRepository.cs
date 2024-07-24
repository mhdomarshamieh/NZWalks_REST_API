using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Reflection.Metadata.Ecma335;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _context;

        public SQLRegionRepository(NZWalksDbContext context)
        {
            _context = context;
        }

        public async Task<List<Region>> GetAllAsync()
        {

            List<Region> regionDomainModelList = await _context.Regions.ToListAsync();

            return regionDomainModelList;

        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            var regionDomainModel = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

            return regionDomainModel == null ? null : regionDomainModel;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegionDomainModel = await _context.Regions.FirstOrDefaultAsync(x =>x.Id == id);

            if(existingRegionDomainModel == null)
            {
                return null;
            }

            existingRegionDomainModel.Name = region.Name;
            existingRegionDomainModel.Code = region.Code;
            existingRegionDomainModel.RegionImageUrl = region.RegionImageUrl;

            await _context.SaveChangesAsync();

            return existingRegionDomainModel;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion =  await _context.Regions.FirstOrDefaultAsync(x =>x.Id == id);

            if(existingRegion == null)
            {
                return null;
            }

            _context.Regions.Remove(existingRegion);
            await _context.SaveChangesAsync();
            return existingRegion;
        }
    }
}
