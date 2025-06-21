using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.DTO
{
    public class DisplayConfigDTO
    {
        // Methods
        public DisplayConfigDTO() { }

        // Properties     
        public float? WidthRectangle { get; set; }
        public float? HeightRectangle { get; set; }
        public int? TextPosition { get; set; }
        public int? TypeDisplay { get; set; }
        public int? SizeFont { get; set; }
        public bool? IsDisplaySignature { get; set; }
        public string FormatRectangleText { get; set; }
        public string[] Titles { get; set; }
        public string Location { get; set; }
        private int? alignment { get; set; }
        public int? Alignment { get { return alignment ?? 2; } set { alignment = value; } }
        public bool? IsBold { get; set; }
        public bool? IsItalic { get; set; }
        public bool? IsUnderlined { get; set; }
        public string FontName { get; set; }
    }
}
