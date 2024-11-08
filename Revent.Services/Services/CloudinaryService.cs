using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Revent.Common.CommonModels;
using Revent.Services.IServices;

namespace Revent.Services.Services
{
    public class CloudinaryService : ICloudinaryService
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

        public async Task<(string?, long?)> UploadImage(IFormFile file, int quality)
        {
            var transformation = new Transformation()
              .Quality(quality);  // Compress image to 70% quality (you can adjust this value)

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "profile_pictures"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                return (null, null);

            long imageSize = uploadResult.Bytes; // Cloudinary returns the image size in bytes

            // Return both the URL and the image size
            return (uploadResult.SecureUrl.ToString(), imageSize);
        }

        public async Task<List<CloudinaryImageUploadResponse>> UploadImages(List<IFormFile> files, int quality)
        {
            var transformation = new Transformation()
              .Quality(quality);  // Compress image to 70% quality (you can adjust this value)

            List<CloudinaryImageUploadResponse> uploadedFiles = new List<CloudinaryImageUploadResponse>();

            foreach (var file in files)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "profile_pictures"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    return null;

                long imageSize = uploadResult.Bytes; // Cloudinary returns the image size in bytes

                uploadedFiles.Add(new CloudinaryImageUploadResponse { Url = uploadResult.SecureUrl.ToString(),Size = imageSize});
            }

            // Return both the URL and the image size
            return uploadedFiles;
        }
    }
}
