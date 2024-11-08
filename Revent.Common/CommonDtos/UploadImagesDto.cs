using Microsoft.AspNetCore.Http;
using Revent.Common.CustomValidationAttributes;

namespace Revent.Common.CommonDtos
{
    public class UploadImagesDto
    {
        [AllowedExtensionForPicturesList(new[] { ".jpg", ".jpeg", ".png", ".gif" })]
        public List<IFormFile> Images { get; set; }
    }
}
