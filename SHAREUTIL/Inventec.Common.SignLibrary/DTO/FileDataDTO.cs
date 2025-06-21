using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Inventec.Common.SignLibrary.DTO
{
    public class FileDataDTO
    {
        // Methods
        public FileDataDTO() { }
        public byte[] BFile { get; set; }
        public int Size { get; set; }
        public MemoryStream Stream { get; set; }
    }
}
