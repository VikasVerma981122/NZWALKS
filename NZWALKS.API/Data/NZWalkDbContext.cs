using Microsoft.EntityFrameworkCore;
using NZWALKS.API.Models.Domain;

namespace NZWALKS.API.Data
{
    public class NZWalkDbContext : DbContext
    {
        public NZWalkDbContext(DbContextOptions<NZWalkDbContext> dbContextOptions): base(dbContextOptions) 
        {
                
        }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var difficulties = new List<Difficulty>()// list of domainn model
            { 
             new Difficulty()
             {
                 Id= Guid.Parse("4acaa618-b1f7-4af1-8797-876c9e79714f"),
                 Name="Easy"
             },
             new Difficulty()
             {
                 Id= Guid.Parse("e24ae8ae-fef3-4275-aa8d-1c25b6b17a9a"),
                 Name="Medium"
             },
             new Difficulty()
             {
                 Id= Guid.Parse("46fb5b83-dadb-446a-adc6-f378e7ffa515"),
                 Name="Hard"
             }
            };
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            var regions = new List<Region>()
            {
             new Region()
             {
              Id = Guid.Parse("108dbd32-6e7c-49b4-b5b6-80f398e2bac2"),
              Code="AKL",
              Name="Auckland",
              RegionImageUrl="https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
             },
              new Region()
             {
              Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
              Code="WLN",
              Name="Wellington",
              RegionImageUrl="https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
             },
               new Region()
             {
              Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
              Code="NEL",
              Name="Nelson",
              RegionImageUrl="https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
             },
               new Region()
             {
              Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
              Code="BOP",
              Name="Bay of Plenty",
              RegionImageUrl=null
             }
            };
            modelBuilder.Entity<Region>().HasData(regions);
        }
            
    }
}
