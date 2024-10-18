using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Revent.Services.IServices;

namespace Revent.Services.Services
{
    public class CloudinaryService:ICloudinaryService
    {
        private readonly Cloudinary _cloudinary; 
        public CloudinaryService(Cloudinary cloudinary) 
        {
            _cloudinary = cloudinary;
        }

        public async Task<string?> UploadImage(IFormFile file)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "profile_pictures" 
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            return uploadResult.SecureUrl.ToString();
        }
    }
}
