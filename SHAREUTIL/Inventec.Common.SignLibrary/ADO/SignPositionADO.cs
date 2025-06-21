using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.ADO
{
    class SignPositionADO
    {
        public Rectangle Reactanle { get; set; }
        public int PageNUm { get; set; }
        public string Text { get; set; }
        public int TypeDisplay { get; set; }
        public bool? IsDisplaySignature { get; set; }
        public int SizeFont { get; set; }
        public float WidthRectangle { get; set; }
        public float HeightRectangle { get; set; }
        public Inventec.Common.SignFile.Constans.TEXT_POSITON TextPosition { get; set; }
        public string Signer { get; set; }
        public List<SignPositionADO> SignPositionAutos { get; set; }
    }
}
