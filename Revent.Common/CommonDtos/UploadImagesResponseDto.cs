using Revent.Common.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CommonDtos
{
    public class UploadImagesResponseDto
    {
        public List<CloudinaryImageUploadResponse>  UplaodResult { get; set; }
    }
}
