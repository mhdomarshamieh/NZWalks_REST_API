using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _context;

        public SQLWalkRepository(NZWalksDbContext context) { 
            _context = context;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _context.Walks.AddAsync(walk);
            await _context.SaveChangesAsync();
            return walk;
        }

        

        public async Task<List<Walk>> GetAllAsync()
        {
            return await _context.Walks.
                        Include(nameof(Difficulty)).
                        Include(nameof(Region)).
                        ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _context.Walks.
                Include(nameof(Region)).
                Include(nameof(Difficulty)).
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
           var existingWalk = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if(existingWalk == null) {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.Description = walk.Description;

            await _context.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalkDomainModel = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if( existingWalkDomainModel == null)
            {
                return null;
            }

            _context.Walks.Remove(existingWalkDomainModel);
            await _context.SaveChangesAsync();
            return existingWalkDomainModel;
        }
    }
}
