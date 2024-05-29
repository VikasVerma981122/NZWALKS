using Microsoft.EntityFrameworkCore;
using NZWALKS.API.Data;
using NZWALKS.API.Models.Domain;

namespace NZWALKS.API.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalkDbContext nZWalkDbContext;

        public SqlRegionRepository(NZWalkDbContext nZWalkDbContext)
        {
            this.nZWalkDbContext = nZWalkDbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await nZWalkDbContext.AddAsync(region);
            await nZWalkDbContext.SaveChangesAsync();
            return region;
            
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
           var existingregion = await nZWalkDbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            if (existingregion == null) 
            {
                return null;
            }
            nZWalkDbContext.Regions.Remove(existingregion);
            await nZWalkDbContext.SaveChangesAsync();
            return existingregion;

        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await nZWalkDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetByIdAsync(Guid id)
        {
            return await nZWalkDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingregion = await nZWalkDbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            if (existingregion == null) 
            { 
             return null;
            }
            existingregion.Code = region.Code;
            existingregion.Name = region.Name;
            existingregion.RegionImageUrl = region.RegionImageUrl;
            await nZWalkDbContext.SaveChangesAsync();
            return existingregion;
        }
    }
}
