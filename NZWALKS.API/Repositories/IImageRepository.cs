using NZWALKS.API.Models.Domain;
using System.Net;

namespace NZWALKS.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image>Upload(Image image);
    }
}
