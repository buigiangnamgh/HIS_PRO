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
			Alignment = alignment;
			IsBold = isBold;
			IsItalic = isItalic;
			IsUnderlined = isUnderlined;
			if (fontName != null)
			{
				FontName = fontName;
			}
		}
	}
}
