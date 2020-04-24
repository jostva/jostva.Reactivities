using jostva.Reactivities.application.Photos;
using Microsoft.AspNetCore.Http;

namespace jostva.Reactivities.application.Interfaces
{
    public interface IPhotoAccessor
    {
        PhotoUploadResult AddPhoto(IFormFile formFile);

        string DeletePhoto(string publicId);
    }
}