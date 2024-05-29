using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWALKS.API.Models.Domain;
using NZWALKS.API.Models.DTO;
using NZWALKS.API.Repositories;

namespace NZWALKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult>Upload([FromForm]ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if(ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };

                //Implementation for Image Repository..//...

                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
              return BadRequest(ModelState);
            
        }
        private void ValidateFileUpload (ImageUploadRequestDto request) 
        { 
         var allowedExtensions = new string[] {".jpg",".jpeg",".png"};
        if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
        {
         ModelState.AddModelError("file", "Unsupported file extension");
        }
        if(request.File.Length > 10485760)
        {
         ModelState.AddModelError("file", "File size is more than 10 MB,please upload a smaller size file..");
        }
        }
    }
}
