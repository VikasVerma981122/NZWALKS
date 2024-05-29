using Microsoft.EntityFrameworkCore;
using NZWALKS.API.Data;
using NZWALKS.API.Models.Domain;

namespace NZWALKS.API.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly NZWalkDbContext nZWalkDbContext;

        public SqlWalkRepository(NZWalkDbContext nZWalkDbContext)
        {
            this.nZWalkDbContext = nZWalkDbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await nZWalkDbContext.Walks.AddAsync(walk);
            await nZWalkDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
           var existingWalk = await nZWalkDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
           if (existingWalk == null)
           {
            return null;
           }
            nZWalkDbContext.Walks.Remove(existingWalk);
            await nZWalkDbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? SortBy = null,
            bool isAscending = true , int pageNumber = 1, int pageSize = 1000)
        {
            var walks = nZWalkDbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();
            // return await nZWalkDbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();
            //Filtering 

            if(string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery)==false) 
            {
                if (filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }
            //Sorting
            if (string.IsNullOrWhiteSpace(SortBy) == false)
            {
              if(SortBy.Equals("Name",StringComparison.OrdinalIgnoreCase)) 
              { 
                  walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x =>x.Name);
              }
              else if(SortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
              {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
              }
            }

            //Pagination
            var Skipresult = (pageNumber - 1) * pageSize;
            return await walks.Skip(Skipresult).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await nZWalkDbContext.Walks.
            Include("Region").
            Include("Difficulty").
            FirstOrDefaultAsync(x => x.Id == id);      
        }

        public async Task<Walk?> UpdateAsync(Guid id,Walk walk)
        {
            var existingwalk = await nZWalkDbContext.Walks.FirstOrDefaultAsync(x=>x.Id == id);
            if (existingwalk == null) 
            {
                return null;
            }
            existingwalk.Name = walk.Name;
            existingwalk.Description = walk.Description;
            existingwalk.LengthInKm = walk.LengthInKm;
            existingwalk.ImageUrl = walk.ImageUrl;
            existingwalk.DifficultyId = walk.DifficultyId;
            existingwalk.RegionId = walk.RegionId;

            await nZWalkDbContext.SaveChangesAsync();
            
            return existingwalk;
            
        }
    }
}
