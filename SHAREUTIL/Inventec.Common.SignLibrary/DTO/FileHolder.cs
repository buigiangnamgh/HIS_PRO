using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.DTO
{
    public class FileHolder
    {
        public FileHolder(){ }
        public FileHolder(MemoryStream content, string fileName)
        {
            this.Content = content;
            this.FileName = fileName;
        }

        public MemoryStream Content { get; set; }
        public string FileName { get; set; }
    }
}
