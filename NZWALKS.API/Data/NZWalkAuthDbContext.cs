using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWALKS.API.Data
{
    public class NZWalkAuthDbContext : IdentityDbContext
    {
        public NZWalkAuthDbContext(DbContextOptions<NZWalkAuthDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var ReaderRoleId = "251e8653-d1e5-4660-87c7-ce762911b2bd";
            var WriterRoleId = "a09204a0-f831-4201-9997-df75fdd6c4db";

            var roles = new List<IdentityRole>()
            {
             new IdentityRole
             {
                 Id = ReaderRoleId,
                 ConcurrencyStamp = ReaderRoleId,
                 Name = "Reader",
                 NormalizedName = "Reader".ToUpper()
             },
             new IdentityRole 
             {
                 Id = WriterRoleId,
                 ConcurrencyStamp = WriterRoleId,
                 Name = "Writter",
                 NormalizedName = "Writter".ToUpper()
             }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
