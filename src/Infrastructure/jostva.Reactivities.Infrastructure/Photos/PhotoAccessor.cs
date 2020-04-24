using Castle.DynamicProxy.Contributors;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.application.Photos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace jostva.Reactivities.Infrastructure.Photos
{
    public class PhotoAccessor : IPhotoAccessor
    {
        private readonly Cloudinary cloudinary;


        public PhotoAccessor(IOptions<CloudinarySettings> config)
        {
            Account account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            cloudinary = new Cloudinary(account);
        }


        public PhotoUploadResult AddPhoto(IFormFile formFile)
        {
            ImageUploadResult uploadResult = new ImageUploadResult();

            if (formFile.Length > 0)
            {
                using (Stream stream = formFile.OpenReadStream())
                {
                    ImageUploadParams uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(formFile.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return new PhotoUploadResult()
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUri.AbsoluteUri
            };
        }


        public string DeletePhoto(string publicId)
        {
            DeletionParams deleteParams = new DeletionParams(publicId);

            DeletionResult result = cloudinary.Destroy(deleteParams);

            return result.Result == "ok" ? result.Result : null;
        }
    }
}