
namespace Inventec.Common.SignFile
{
    public class FontConfig
    {
        public ALIGNMENT_OPTION Alignment { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderlined { get; set; }
        public string FontName { get; set; }

        public FontConfig()
        {
        }

        public FontConfig(ALIGNMENT_OPTION alignment, bool isBold, bool isItalic, bool isUnderlined, string fontName = null)
        {
            this.Alignment = alignment;
            this.IsBold = isBold;
            this.IsItalic = isItalic;
            this.IsUnderlined = isUnderlined;
            if (fontName != null) this.FontName = fontName;
        }
    }

    public enum ALIGNMENT_OPTION
    {
        Default = 0,
        Left = 1,
        Center = 2,
        Right = 3,
        Justify = 4
    }
}
