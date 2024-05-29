using NZWALKS.API.Data;
using NZWALKS.API.Models.Domain;

namespace NZWALKS.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalkDbContext nZWalkDbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, 
        IHttpContextAccessor httpContextAccessor, NZWalkDbContext nZWalkDbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.nZWalkDbContext = nZWalkDbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var localfilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",$"{image.FileName}{image.FileExtension}");

            // Line no: 19,20 responsible for Image Upload..

            using var stream = new FileStream(localfilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            await nZWalkDbContext.Images.AddAsync(image);   
            await nZWalkDbContext.SaveChangesAsync();

            return image;
        }
    }
}
