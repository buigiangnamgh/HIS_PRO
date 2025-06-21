using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.DTO
{
    public class ImageOfPageDTO
    {
        // Methods
        public ImageOfPageDTO() { }

        // Properties   
        public string Path { get; set; }
        public byte[] ImageContent { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public int PageNumber { get; set; }
    }
}
