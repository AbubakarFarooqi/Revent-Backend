using Microsoft.AspNetCore.Http;
using Revent.Common.CustomValidationAttributes;

namespace Revent.Common.CommonDtos
{
    public class UploadImageDto
    {
        [AllowedExtensionForPictures(new[] { ".jpg", ".jpeg", ".png", ".gif" })]
        public IFormFile Picture { get; set; }

    }
}
