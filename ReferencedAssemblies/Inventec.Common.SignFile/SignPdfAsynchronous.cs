using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using com.itextpdf.text.pdf.security;
using Inventec.Common.Logging;
using iTextSharp.text;
using iTextSharp.text.io;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace Inventec.Common.SignFile
{
	public class SignPdfAsynchronous
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass9_0
		{
			public DisplayConfig displayConfig;

			public string strDate;

			public string displayText;
		}

		private const string CRYPT_ALG = "RSA";

		private static string hashAlgorithmCfg;

		internal static string HASH_ALG
		{
			get
			{
				if (hashAlgorithmCfg == null)
				{
					try
					{
						hashAlgorithmCfg = ConfigurationManager.AppSettings["Inventec.Common.SignFile.Hash_Algorithm"];
						if (string.IsNullOrWhiteSpace(hashAlgorithmCfg))
						{
							hashAlgorithmCfg = "SHA1";
						}
					}
					catch (Exception ex)
					{
						LogSystem.Error(ex);
						hashAlgorithmCfg = "SHA1";
					}
				}
				return hashAlgorithmCfg;
			}
		}

		public List<byte[]> CreateHash(string inFile, string tempFile, string fileName, X509Certificate[] chain, DisplayConfig displayConfig)
		{
			if (!EmptySignature(inFile, tempFile, fileName, displayConfig, chain[0]))
			{
				return null;
			}
			return PreSign(tempFile, fileName, chain, displayConfig);
		}

		internal static void ProcessFontSizeFit(DisplayConfig displayConfig)
		{
			int newSizeFont = 0;
			if (displayConfig.WidthRectangle > 0f)
			{
				if (displayConfig.WidthRectangle <= 30f && displayConfig.SizeFont >= 2)
				{
					newSizeFont = 1;
				}
				if (displayConfig.WidthRectangle <= 40f && displayConfig.SizeFont >= 3)
				{
					newSizeFont = 2;
				}
				else if (displayConfig.WidthRectangle <= 60f && displayConfig.SizeFont >= 4)
				{
					newSizeFont = 3;
				}
				else if (displayConfig.WidthRectangle <= 80f && displayConfig.SizeFont >= 5)
				{
					newSizeFont = 4;
				}
				else if (displayConfig.WidthRectangle <= 120f && displayConfig.SizeFont >= 6)
				{
					newSizeFont = 5;
				}
				else if (displayConfig.WidthRectangle <= 160f && displayConfig.SizeFont >= 7)
				{
					newSizeFont = 6;
				}
			}
			if (displayConfig.SizeFont != newSizeFont && newSizeFont > 0)
			{
				displayConfig.SizeFont = newSizeFont;
				LogSystem.Debug("Kiem tra SizeFont cua vung chu ky, neu do rong cua vung ky duoc cau hinh  khong phu hop voi SizeFont thi tu dong dieu chinh cho phu hop____" + LogUtil.TraceData("oldSizeFont", (object)displayConfig.SizeFont) + LogUtil.TraceData(LogUtil.GetMemberName<int>((Expression<Func<int>>)(() => newSizeFont)), (object)newSizeFont));
			}
		}

		public static float ProcessHeightPlus(float widthImagePercent, DisplayConfig displayConfig)
		{
			float result = 0f;
			if (widthImagePercent == 100f)
			{
				if (displayConfig.WidthRectangle <= 30f)
				{
					result = 5f;
				}
				result = ((displayConfig.WidthRectangle <= 40f) ? 5f : ((displayConfig.WidthRectangle <= 60f) ? 10f : ((displayConfig.WidthRectangle <= 80f) ? 15f : ((displayConfig.WidthRectangle <= 100f) ? 25f : ((!(displayConfig.WidthRectangle <= 120f)) ? 40f : 30f)))));
			}
			return result;
		}

		public static float ProcessHeightPlus(float widthImagePercent, float WidthRectangle)
		{
			float result = 0f;
			if (widthImagePercent == 100f)
			{
				if (WidthRectangle <= 30f)
				{
					result = 5f;
				}
				result = ((WidthRectangle <= 40f) ? 5f : ((WidthRectangle <= 60f) ? 10f : ((WidthRectangle <= 80f) ? 15f : ((WidthRectangle <= 100f) ? 25f : ((!(WidthRectangle <= 120f)) ? 40f : 30f)))));
			}
			return result;
		}

		public bool EmptySignature(string inFile, string outFile, string fieldName, DisplayConfig displayConfig, X509Certificate cert)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bac: Expected O, but got Unknown
			//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bcc: Expected O, but got Unknown
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Expected O, but got Unknown
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Expected O, but got Unknown
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Expected O, but got Unknown
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Expected O, but got Unknown
			//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7c: Expected O, but got Unknown
			//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac5: Expected O, but got Unknown
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05af: Expected O, but got Unknown
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Expected O, but got Unknown
			//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b5: Expected O, but got Unknown
			//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bc: Expected O, but got Unknown
			//IL_0626: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Expected O, but got Unknown
			//IL_0665: Unknown result type (might be due to invalid IL or missing references)
			//IL_066c: Expected O, but got Unknown
			//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ab: Expected O, but got Unknown
			//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ea: Expected O, but got Unknown
			//IL_071f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0726: Expected O, but got Unknown
			//IL_091b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0922: Expected O, but got Unknown
			//IL_0922: Unknown result type (might be due to invalid IL or missing references)
			//IL_0929: Expected O, but got Unknown
			//IL_075b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0762: Expected O, but got Unknown
			PdfReader val = null;
			FileStream fileStream = null;
			bool success;
			try
			{
				val = new PdfReader(inFile);
				int numberOfPages = val.NumberOfPages;
				int num = displayConfig.NumberPageSign;
				if (num < 1 || num > numberOfPages)
				{
					num = 1;
				}
				fileStream = new FileStream(outFile, FileMode.Create);
				bool IsRebuilt = val.IsRebuilt();
				LogSystem.Debug("EmptySignature: " + LogUtil.TraceData(LogUtil.GetMemberName<bool>((Expression<Func<bool>>)(() => IsRebuilt)), (object)IsRebuilt));
				PdfSignatureAppearance val2 = null;
				if (IsRebuilt)
				{
					val.Catalog.Remove(PdfName.PERMS);
					val.RemoveUsageRights();
					val2 = PdfStamper.CreateSignature(val, (Stream)fileStream, '\0', (string)null).SignatureAppearance;
				}
				else
				{
					val2 = PdfStamper.CreateSignature(val, (Stream)fileStream, '\0', (string)null, true).SignatureAppearance;
				}
				DateTime signDate = displayConfig.SignDate;
				if ("".Equals(displayConfig.Contact))
				{
					displayConfig.Contact = SharedUtils.GetCN(cert);
				}
				val2.Contact = displayConfig.Contact;
				val2.SignDate = signDate;
				val2.Reason = displayConfig.Reason;
				val2.Location = displayConfig.Location;
				val2.Certificate = cert;
				string strDate = string.Format(displayConfig.DateFormatstring, signDate);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<DisplayConfig>((Expression<Func<DisplayConfig>>)(() => displayConfig)), (object)displayConfig));
				if (displayConfig.IsDisplaySignature)
				{
					float num2 = displayConfig.CoorXRectangle - displayConfig.WidthRectangle / 2f;
					float num3 = displayConfig.CoorYRectangle - displayConfig.HeightRectangle / 2f;
					float widthRectangle = displayConfig.WidthRectangle;
					float heightRectangle = displayConfig.HeightRectangle;
					ProcessFontSizeFit(displayConfig);
					float num4 = 0f;
					float num5 = 0f;
					Image val3 = null;
					if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT)
					{
						if (!string.IsNullOrEmpty(displayConfig.PathImage) && File.Exists(displayConfig.PathImage))
						{
							val3 = Image.GetInstance(displayConfig.PathImage);
						}
						else if (displayConfig.BImage != null)
						{
							val3 = Image.GetInstance(displayConfig.BImage);
						}
					}
					else if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP)
					{
						if (!string.IsNullOrEmpty(displayConfig.PathImage) && File.Exists(displayConfig.PathImage))
						{
							val3 = Image.GetInstance(displayConfig.PathImage);
						}
						else if (displayConfig.BImage != null)
						{
							val3 = Image.GetInstance(displayConfig.BImage);
						}
					}
					if (displayConfig.SignType == Constans.SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD)
					{
						Rectangle val4 = new Rectangle(num2, num3, num2 + widthRectangle + num4, num3 + heightRectangle + num5);
						val2.SetVisibleSignature(val4, num, fieldName);
					}
					else
					{
						val2.SetVisibleSignature(fieldName);
					}
					if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP)
					{
						float totalWidth = widthRectangle;
						PdfTemplate layer = val2.GetLayer(2);
						float left = layer.BoundingBox.Left;
						float bottom = layer.BoundingBox.Bottom;
						float width = layer.BoundingBox.Width;
						float height = layer.BoundingBox.Height;
						ColumnText val5 = new ColumnText((PdfContentByte)(object)layer);
						PdfPCell val6 = new PdfPCell();
						if (val3 != null)
						{
							val3.Alignment = 1;
							float plusH = ProcessHeightPlus(100f, displayConfig);
							val3.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, val3, displayConfig.SignaltureImageWidth, 100f, plusH);
							val6.AddElement((IElement)(object)val3);
							val6.HorizontalAlignment = 1;
							val6.VerticalAlignment = 5;
							((Rectangle)val6).Border = 0;
							val6.MinimumHeight = heightRectangle;
							LogSystem.Info(LogUtil.TraceData("instance.WidthPercentage", (object)val3.WidthPercentage) + LogUtil.TraceData("instance.Width", (object)((Rectangle)val3).Width) + LogUtil.TraceData("displayConfig.SignaltureImageWidth", (object)displayConfig.SignaltureImageWidth));
						}
						PdfPTable val7 = new PdfPTable(1);
						val7.TotalWidth = totalWidth;
						val7.LockedWidth = true;
						val7.AddCell(val6);
						val5.AddElement((IElement)(object)val7);
						val5.SetSimpleColumn(left, bottom, width, height);
						val5.Alignment = 1;
						val5.Go();
					}
					else if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT)
					{
						string displayText = GetDisplayText(displayConfig, strDate);
						float widthImagePercent = 0f;
						PdfPTable val8 = null;
						if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100)
						{
							val8 = new PdfPTable(1);
							widthImagePercent = 100f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75)
						{
							val8 = new PdfPTable(new float[2] { 25f, 75f });
							widthImagePercent = 25f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70)
						{
							val8 = new PdfPTable(new float[2] { 30f, 70f });
							widthImagePercent = 30f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60)
						{
							val8 = new PdfPTable(new float[2] { 40f, 60f });
							widthImagePercent = 40f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50)
						{
							val8 = new PdfPTable(new float[2] { 50f, 50f });
							widthImagePercent = 50f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x60x40)
						{
							val8 = new PdfPTable(new float[2] { 60f, 40f });
							widthImagePercent = 40f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x70x30)
						{
							val8 = new PdfPTable(new float[2] { 70f, 30f });
							widthImagePercent = 30f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x75x25)
						{
							val8 = new PdfPTable(new float[2] { 75f, 25f });
							widthImagePercent = 25f;
						}
						PdfTemplate layer2 = val2.GetLayer(2);
						float left2 = layer2.BoundingBox.Left;
						float bottom2 = layer2.BoundingBox.Bottom;
						float width2 = layer2.BoundingBox.Width;
						float height2 = layer2.BoundingBox.Height;
						ColumnText val9 = new ColumnText((PdfContentByte)(object)layer2);
						PdfPCell val10 = new PdfPCell();
						if (val3 != null)
						{
							val3.Alignment = 1;
							float plusH2 = ProcessHeightPlus(widthImagePercent, displayConfig);
							val3.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, val3, displayConfig.SignaltureImageWidth, widthImagePercent, plusH2);
							val10.AddElement((IElement)(object)val3);
							val10.HorizontalAlignment = 1;
							val10.VerticalAlignment = 5;
							((Rectangle)val10).Border = 0;
							LogSystem.Info(LogUtil.TraceData("instance.WidthPercentage", (object)val3.WidthPercentage) + LogUtil.TraceData("instance.Width", (object)((Rectangle)val3).Width) + LogUtil.TraceData("displayConfig.SignaltureImageWidth", (object)displayConfig.SignaltureImageWidth));
						}
						PdfPCell textCell = SignPdfFile.GetTextCell(displayText, displayConfig);
						val8.TotalWidth = widthRectangle;
						val8.LockedWidth = true;
						if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100 || displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75 || displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70 || displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60 || displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50)
						{
							val8.AddCell(val10);
							val8.AddCell(textCell);
						}
						else
						{
							val8.AddCell(textCell);
							val8.AddCell(val10);
						}
						PdfPTable val11 = new PdfPTable(1);
						PdfPCell val12 = new PdfPCell();
						val12.AddElement((IElement)(object)val8);
						val12.HorizontalAlignment = 1;
						val12.VerticalAlignment = 5;
						((Rectangle)val12).Border = 0;
						val12.MinimumHeight = heightRectangle;
						val11.TotalWidth = widthRectangle;
						val11.LockedWidth = true;
						val11.AddCell(val12);
						val9.AddElement((IElement)(object)val11);
						val9.SetSimpleColumn(left2, bottom2, width2, height2);
						val9.Alignment = 1;
						val9.Go();
						LogSystem.Debug(LogUtil.TraceData("instance.Width", (object)((val3 != null) ? ((Rectangle)val3).Width : 0f)) + LogUtil.TraceData("instance.Height", (object)((val3 != null) ? ((Rectangle)val3).Height : 0f)) + LogUtil.TraceData("widthRectangle", (object)widthRectangle) + LogUtil.TraceData("heightRectangle", (object)heightRectangle));
					}
					else if (displayConfig.TypeDisplay == Constans.DISPLAY_RECTANGLE_TEXT)
					{
						PdfTemplate layer3 = val2.GetLayer(2);
						float left3 = layer3.BoundingBox.Left;
						float bottom3 = layer3.BoundingBox.Bottom;
						float width3 = layer3.BoundingBox.Width;
						float height3 = layer3.BoundingBox.Height;
						ColumnText val13 = new ColumnText((PdfContentByte)(object)layer3);
						val13.SetSimpleColumn(left3, bottom3, width3, height3);
						val13.Alignment = 5;
						string displayText2 = GetDisplayText(displayConfig, strDate);
						PdfPCell textCell2 = SignPdfFile.GetTextCell(displayText2, displayConfig);
						textCell2.MinimumHeight = heightRectangle;
						PdfPTable val14 = new PdfPTable(1);
						val14.TotalWidth = widthRectangle;
						val14.HorizontalAlignment = 1;
						val14.LockedWidth = true;
						val14.AddCell(textCell2);
						val14.CompleteRow();
						val13.AddElement((IElement)(object)val14);
						val13.Go();
					}
					LogSystem.Info("EmptySignature____" + LogUtil.TraceData(LogUtil.GetMemberName<int>((Expression<Func<int>>)(() => displayConfig.TypeDisplay)), (object)displayConfig.TypeDisplay));
				}
				else if (displayConfig.SignType == Constans.SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD)
				{
					val2.SetVisibleSignature(new Rectangle(0f, 0f, 0f, 0f), 1, fieldName);
				}
				else
				{
					val2.SetVisibleSignature(fieldName);
				}
				IExternalSignatureContainer val15 = (IExternalSignatureContainer)new ExternalBlankSignatureContainer(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
				MakeSignature.SignExternalContainer(val2, val15, 8192);
				success = true;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				success = false;
			}
			finally
			{
				if (val != null)
				{
					val.Close();
				}
				if (fileStream != null)
				{
					try
					{
						fileStream.Close();
						fileStream.Dispose();
					}
					catch (IOException ex2)
					{
						LogSystem.Warn("Error emptySignature: " + ex2.Message);
					}
				}
			}
			LogSystem.Debug("EmptySignature.____" + LogUtil.TraceData(LogUtil.GetMemberName<bool>((Expression<Func<bool>>)(() => success)), (object)success));
			return success;
		}

		internal static string GetDisplayText(DisplayConfig displayConfig, string strDate)
		{
			_003C_003Ec__DisplayClass9_0 CS_0024_003C_003E8__locals75 = new _003C_003Ec__DisplayClass9_0();
			CS_0024_003C_003E8__locals75.displayConfig = displayConfig;
			CS_0024_003C_003E8__locals75.strDate = strDate;
			CS_0024_003C_003E8__locals75.displayText = string.Empty;
			try
			{
                //LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<DisplayConfig>((Expression<Func<DisplayConfig>>)(() => CS_0024_003C_003E8__locals75.displayConfig)), (object)CS_0024_003C_003E8__locals75.displayConfig) + LogUtil.TraceData(LogUtil.GetMemberName<string>(Expression.Lambda<Func<string>>(Expression.Field(Expression.Constant(CS_0024_003C_003E8__locals75, typeof(_003C_003Ec__DisplayClass9_0)), FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[0])), (object)CS_0024_003C_003E8__locals75.strDate));
				if (CS_0024_003C_003E8__locals75.displayConfig.DisplayText != null && CS_0024_003C_003E8__locals75.displayConfig.DisplayText.Length != 0)
				{
					CS_0024_003C_003E8__locals75.displayText = CS_0024_003C_003E8__locals75.displayConfig.DisplayText;
				}
				else
				{
					List<object> list = new List<object>();
					List<string> list2 = new List<string>();
					string text = "";
					string text2 = "";
					if (!string.IsNullOrEmpty(CS_0024_003C_003E8__locals75.displayConfig.Location) && CS_0024_003C_003E8__locals75.displayConfig.Location.Contains("|"))
					{
						string[] array = CS_0024_003C_003E8__locals75.displayConfig.Location.Split(new string[1] { "|" }, StringSplitOptions.RemoveEmptyEntries);
						if (array != null && array.Length != 0)
						{
							text = array[0];
						}
						if (array != null && array.Length > 1)
						{
							text2 = array[1];
						}
					}
					else
					{
						text = CS_0024_003C_003E8__locals75.displayConfig.Location;
						if (CS_0024_003C_003E8__locals75.displayConfig.Titles != null && CS_0024_003C_003E8__locals75.displayConfig.Titles.Length != 0)
						{
							text2 = CS_0024_003C_003E8__locals75.displayConfig.Titles[0];
						}
					}
					if (CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText == Constans.SIGN_TEXT_FORMAT_3__NO_DATE || CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText == Constans.SIGN_TEXT_FORMAT_USER)
					{
						list.Add(CS_0024_003C_003E8__locals75.displayConfig.Contact.Replace("\\", ""));
						list2.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", list2.Count.ToString()));
						if (!string.IsNullOrEmpty(text2))
						{
							list.Add(text2);
							list2.Add(Constans.SIGN_TEXT_FORMAT_TITLE.Replace("0", list2.Count.ToString()));
						}
						if (CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.HasValue && CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.Value && !string.IsNullOrEmpty(CS_0024_003C_003E8__locals75.displayConfig.Reason))
						{
							list2.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", list2.Count.ToString()));
							list.Add(CS_0024_003C_003E8__locals75.displayConfig.Reason);
						}
						CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText = string.Join("\r\n", list2);
					}
					else if (CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText == Constans.SIGN_TEXT_FORMAT_3__NO_TITLE)
					{
						list.Add(CS_0024_003C_003E8__locals75.displayConfig.Contact.Replace("\\", ""));
						list2.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", list2.Count.ToString()));
						list.Add(CS_0024_003C_003E8__locals75.strDate);
						list2.Add(Constans.SIGN_TEXT_FORMAT_DATE.Replace("0", list2.Count.ToString()));
						if (!string.IsNullOrEmpty(text))
						{
							list.Add(text);
							list2.Add(Constans.SIGN_TEXT_FORMAT_PLACE.Replace("0", list2.Count.ToString()));
						}
						if (CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.HasValue && CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.Value && !string.IsNullOrEmpty(CS_0024_003C_003E8__locals75.displayConfig.Reason))
						{
							list2.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", list2.Count.ToString()));
							list.Add(CS_0024_003C_003E8__locals75.displayConfig.Reason);
						}
						CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText = string.Join("\r\n", list2);
					}
					else
					{
						if (CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText == "2")
						{
							list.Add(CS_0024_003C_003E8__locals75.displayConfig.Contact.Replace("\\", ""));
							list2.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", list2.Count.ToString()));
							if (!string.IsNullOrEmpty(text2))
							{
								list.Add(text2);
								list2.Add(Constans.SIGN_TEXT_FORMAT_TITLE.Replace("0", list2.Count.ToString()));
							}
							if (CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.HasValue && CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.Value && !string.IsNullOrEmpty(CS_0024_003C_003E8__locals75.displayConfig.Reason))
							{
								list2.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", list2.Count.ToString()));
								list.Add(CS_0024_003C_003E8__locals75.displayConfig.Reason);
							}
						}
						else if (CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText == "3")
						{
							list.Add(CS_0024_003C_003E8__locals75.displayConfig.Contact.Replace("\\", ""));
							list2.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", list2.Count.ToString()));
							list.Add(CS_0024_003C_003E8__locals75.strDate);
							list2.Add(Constans.SIGN_TEXT_FORMAT_DATE.Replace("0", list2.Count.ToString()));
							if (!string.IsNullOrEmpty(text))
							{
								list.Add(text);
								list2.Add(Constans.SIGN_TEXT_FORMAT_PLACE.Replace("0", list2.Count.ToString()));
							}
							if (CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.HasValue && CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.Value && !string.IsNullOrEmpty(CS_0024_003C_003E8__locals75.displayConfig.Reason))
							{
								list2.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", list2.Count.ToString()));
								list.Add(CS_0024_003C_003E8__locals75.displayConfig.Reason);
							}
						}
						else if (CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText == "4")
						{
							list.Add(CS_0024_003C_003E8__locals75.displayConfig.Contact.Replace("\\", ""));
							list2.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", list2.Count.ToString()));
							list.Add(CS_0024_003C_003E8__locals75.strDate);
							list2.Add(Constans.SIGN_TEXT_FORMAT_DATE.Replace("0", list2.Count.ToString()));
							if (!string.IsNullOrEmpty(text2))
							{
								list.Add(text2);
								list2.Add(Constans.SIGN_TEXT_FORMAT_TITLE.Replace("0", list2.Count.ToString()));
							}
							if (CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.HasValue && CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.Value && !string.IsNullOrEmpty(CS_0024_003C_003E8__locals75.displayConfig.Reason))
							{
								list2.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", list2.Count.ToString()));
								list.Add(CS_0024_003C_003E8__locals75.displayConfig.Reason);
							}
						}
						else if (CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText == "5")
						{
							list.Add(CS_0024_003C_003E8__locals75.displayConfig.Contact.Replace("\\", ""));
							list2.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", list2.Count.ToString()));
							if (!string.IsNullOrEmpty(text))
							{
								list.Add(text);
								list2.Add(Constans.SIGN_TEXT_FORMAT_PLACE.Replace("0", list2.Count.ToString()));
							}
							if (!string.IsNullOrEmpty(text2))
							{
								list.Add(text2);
								list2.Add(Constans.SIGN_TEXT_FORMAT_TITLE.Replace("0", list2.Count.ToString()));
							}
							if (CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.HasValue && CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.Value && !string.IsNullOrEmpty(CS_0024_003C_003E8__locals75.displayConfig.Reason))
							{
								list2.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", list2.Count.ToString()));
								list.Add(CS_0024_003C_003E8__locals75.displayConfig.Reason);
							}
						}
                        else if (CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText == "6")
                        {
                            // gộp chức danh và tên người ký thành 1 chuỗi, không có tiêu đề
                            string titleAndName = string.Empty;
                            if (!String.IsNullOrEmpty(text2))
                            {
                                titleAndName = text2.Trim() + " ";
                            }
                            titleAndName += CS_0024_003C_003E8__locals75.displayConfig.Contact.Replace("\\", "").Trim();
                            list.Add(titleAndName);
                            // định nghĩa format chỉ có 1 tham số, không có tiêu đề
                            list2.Add("{0}");
                        }
                        else if (CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText == "7") 
                        {
                            list.Add(CS_0024_003C_003E8__locals75.displayConfig.Contact.Replace("\\", ""));
                            list2.Add("{0}");

                        }
                        else
                        {
                            list.Add(CS_0024_003C_003E8__locals75.displayConfig.Contact.Replace("\\", ""));
                            list2.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", list2.Count.ToString()));
                            list.Add(CS_0024_003C_003E8__locals75.strDate);
                            list2.Add(Constans.SIGN_TEXT_FORMAT_DATE.Replace("0", list2.Count.ToString()));
                            if (!string.IsNullOrEmpty(text))
                            {
                                list.Add(text);
                                list2.Add(Constans.SIGN_TEXT_FORMAT_PLACE.Replace("0", list2.Count.ToString()));
                            }
                            if (!string.IsNullOrEmpty(text2))
                            {
                                list.Add(text2);
                                list2.Add(Constans.SIGN_TEXT_FORMAT_TITLE.Replace("0", list2.Count.ToString()));
                            }
                            if (CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.HasValue && CS_0024_003C_003E8__locals75.displayConfig.IsDisplaySignNote.Value && !string.IsNullOrEmpty(CS_0024_003C_003E8__locals75.displayConfig.Reason))
                            {
                                list2.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", list2.Count.ToString()));
                                list.Add(CS_0024_003C_003E8__locals75.displayConfig.Reason);
                            }
                        }
						CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText = string.Join("\r\n", list2);
					}
					CS_0024_003C_003E8__locals75.displayText = string.Format(CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText, list.ToArray());
				}
				LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText)), (object)CS_0024_003C_003E8__locals75.displayConfig.FormatRectangleText) + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => CS_0024_003C_003E8__locals75.displayText)), (object)CS_0024_003C_003E8__locals75.displayText));
			}
			catch (Exception ex)
			{
				CS_0024_003C_003E8__locals75.displayText = Constans.SIGN_TEXT_FORMAT_3_1;
				LogSystem.Warn("Error GetDisplayText: " + ex.Message);
			}
			return CS_0024_003C_003E8__locals75.displayText;
		}

		public bool EmptySignatureTable(string inFile, string outFile, string fieldName, DisplayConfig displayConfig, X509Certificate cert)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Expected O, but got Unknown
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Expected O, but got Unknown
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Expected O, but got Unknown
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Expected O, but got Unknown
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Expected O, but got Unknown
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_036d: Expected O, but got Unknown
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Expected O, but got Unknown
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Expected O, but got Unknown
			PdfReader val = null;
			FileStream fileStream = null;
			bool result;
			try
			{
				val = new PdfReader(inFile);
				AcroFields acroFields = val.AcroFields;
				int num = 1;
				float[] array = new float[displayConfig.MaxPageSign];
				foreach (string signatureName in acroFields.GetSignatureNames())
				{
                    IList<AcroFields.FieldPosition> fieldPositions = acroFields.GetFieldPositions(signatureName);
					int page = fieldPositions[0].page;
					if (page > num)
					{
						num = page;
					}
					float height = fieldPositions[0].position.Height;
					array[page] += height;
				}
				Rectangle pageSize = val.GetPageSize(num);
				float height2 = pageSize.Height;
				float marginRight = displayConfig.MarginRight;
				float num2 = pageSize.Width - displayConfig.MarginRight * 2f;
				fileStream = new FileStream(outFile, FileMode.Create);
				PdfSignatureAppearance signatureAppearance = PdfStamper.CreateSignature(val, (Stream)fileStream, '\0', (string)null, true).SignatureAppearance;
				if ("".Equals(displayConfig.Contact))
				{
					displayConfig.Contact = SharedUtils.GetCN(cert);
				}
				signatureAppearance.Contact = displayConfig.Contact;
				signatureAppearance.Reason = displayConfig.Reason;
				signatureAppearance.Location = displayConfig.Location;
				DateTime signDate = displayConfig.SignDate;
				signatureAppearance.SignDate = signDate;
				PdfPTable val2 = new PdfPTable(displayConfig.WidthsPercen.Length);
				val2.SetWidths(displayConfig.WidthsPercen);
				val2.WidthPercentage = 100f;
				val2.TotalWidth = num2;
				for (int i = 0; i < displayConfig.TextArray.Length; i++)
				{
					Paragraph val3 = new Paragraph(displayConfig.TextArray[i], SignPdfFile.GetFontByConfig(displayConfig))
					{
						Alignment = displayConfig.AlignmentArray[i]
					};
					PdfPCell val4 = new PdfPCell();
					val4.AddElement((IElement)(object)val3);
					val2.AddCell(val4);
				}
				float totalHeight = val2.TotalHeight;
				float num3 = height2 - array[num] - displayConfig.MarginTop - totalHeight - displayConfig.HeightTitle;
				Rectangle val5 = null;
				IExternalSignatureContainer val6 = null;
				if (num3 < displayConfig.MarginBottom)
				{
					if (num >= displayConfig.TotalPageSign)
					{
						val5 = new Rectangle(0f, 0f, 0f, 0f);
						signatureAppearance.SetVisibleSignature(val5, num, fieldName);
						val6 = (IExternalSignatureContainer)new ExternalBlankSignatureContainer(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
						MakeSignature.SignExternalContainer(signatureAppearance, val6, 8192);
						val.Close();
						return true;
					}
					num++;
					num3 = height2 - displayConfig.MarginTop - totalHeight - displayConfig.HeightTitle;
				}
				val5 = new Rectangle(marginRight, num3, marginRight + num2, num3 + totalHeight);
				signatureAppearance.SetVisibleSignature(val5, num, fieldName);
				PdfTemplate layer = signatureAppearance.GetLayer(0);
				float left = layer.BoundingBox.Left;
				float bottom = layer.BoundingBox.Bottom;
				float width = layer.BoundingBox.Width;
				float height3 = layer.BoundingBox.Height;
				ColumnText val7 = new ColumnText((PdfContentByte)(object)signatureAppearance.GetLayer(2));
				val7.SetSimpleColumn(left, bottom, left + width, bottom + height3);
				val7.AddElement((IElement)(object)val2);
				val7.Go();
				val6 = (IExternalSignatureContainer)new ExternalBlankSignatureContainer(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
				MakeSignature.SignExternalContainer(signatureAppearance, val6, 8192);
				result = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn("Error emptySignatureTable: " + ex.Message);
				result = false;
			}
			finally
			{
				if (val != null)
				{
					val.Close();
				}
				if (fileStream != null)
				{
					try
					{
						fileStream.Close();
					}
					catch (IOException ex2)
					{
						LogSystem.Warn("Error emptySignatureTable: " + ex2.Message);
					}
				}
			}
			return result;
		}

		public bool InsertSignature(string inFile, string outFile, string fieldName, byte[] hash, byte[] extSignature, X509Certificate[] chain, DateTime signDate, DisplayConfig displayConfig, TimestampConfig timestampConfig)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Expected O, but got Unknown
			PdfReader val = null;
			FileStream fileStream = null;
			bool result;
			try
			{
				val = new PdfReader(inFile);
				fileStream = new FileStream(outFile, FileMode.Append);
				AcroFields acroFields = val.AcroFields;
				PdfDictionary signatureDictionary = acroFields.GetSignatureDictionary(fieldName);
				if (signatureDictionary == null)
				{
					LogSystem.Warn("No field");
					return false;
				}
				if (!acroFields.SignatureCoversWholeDocument(fieldName))
				{
					LogSystem.Warn("Not the last signature");
					return false;
				}
				PdfArray asArray = signatureDictionary.GetAsArray(PdfName.BYTERANGE);
				long[] array = asArray.AsLongArray();
				if (asArray.Size != 4 || array[0] != 0)
				{
					LogSystem.Warn("Single exclusion space supported");
					return false;
				}
				IRandomAccessSource val2 = val.SafeFile.CreateSourceView();
				string text = ((displayConfig != null && !string.IsNullOrEmpty(displayConfig.HashAlgorithm)) ? displayConfig.HashAlgorithm : HASH_ALG);
				PdfPKCS7 val3 = new PdfPKCS7((ICipherParameters)null, (ICollection<X509Certificate>)chain, text, false);
				val3.SetExternalDigest(extSignature, (byte[])null, "RSA");
				TSAClientBouncyCastle val4 = null;
				if (timestampConfig.UseTimestamp)
				{
					val4 = new TSAClientBouncyCastle(timestampConfig.TsaUrl, timestampConfig.TsaAcc, timestampConfig.TsaPass);
				}
				byte[] encodedPKCS = val3.GetEncodedPKCS7(hash, signDate, (ITSAClient)(object)val4, (byte[])null, (ICollection<byte[]>)null, (CryptoStandard)0);
				int num = (int)(array[2] - array[1]) - 2;
				if ((num & 1) != 0)
				{
					LogSystem.Warn("Gap is not a multiple of 2");
					return false;
				}
				num /= 2;
				if (num < encodedPKCS.Length)
				{
					LogSystem.Warn("Not enough space");
					return false;
				}
				StreamUtil.CopyBytes(val2, 0L, array[1] + 1, (Stream)fileStream);
				ByteBuffer val5 = new ByteBuffer(num * 2);
				byte[] array2 = encodedPKCS;
				foreach (byte b in array2)
				{
					val5.AppendHex(b);
				}
				int num2 = (num - encodedPKCS.Length) * 2;
				for (int j = 0; j < num2; j++)
				{
					val5.Append((byte)48);
				}
				val5.WriteTo((Stream)fileStream);
				StreamUtil.CopyBytes(val2, array[2] - 1, array[3] + 1, (Stream)fileStream);
				val2.Close();
				((Stream)(object)val5).Close();
				result = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn("Error insertSignature: " + ex.Message);
				result = false;
			}
			finally
			{
				if (fileStream != null)
				{
					try
					{
						fileStream.Close();
						fileStream.Dispose();
					}
					catch (IOException ex2)
					{
						LogSystem.Warn("Error insertSignature: " + ex2.Message);
					}
				}
				if (val != null)
				{
					val.Close();
				}
			}
			return result;
		}

		public bool InsertSignature(string inFile, Stream outStream, string fieldName, byte[] hash, byte[] extSignature, X509Certificate[] chain, DateTime signDate, DisplayConfig displayConfig, TimestampConfig timestampConfig)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected O, but got Unknown
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			PdfReader val = null;
			bool result;
			try
			{
				val = new PdfReader(inFile);
				AcroFields acroFields = val.AcroFields;
				PdfDictionary signatureDictionary = acroFields.GetSignatureDictionary(fieldName);
				if (signatureDictionary == null)
				{
					LogSystem.Warn("No field");
					return false;
				}
				if (!acroFields.SignatureCoversWholeDocument(fieldName))
				{
					LogSystem.Warn("Not the last signature");
					return false;
				}
				PdfArray asArray = signatureDictionary.GetAsArray(PdfName.BYTERANGE);
				long[] array = asArray.AsLongArray();
				if (asArray.Size != 4 || array[0] != 0)
				{
					LogSystem.Warn("Single exclusion space supported");
					return false;
				}
				IRandomAccessSource val2 = val.SafeFile.CreateSourceView();
				string text = ((displayConfig != null && !string.IsNullOrEmpty(displayConfig.HashAlgorithm)) ? displayConfig.HashAlgorithm : HASH_ALG);
				PdfPKCS7 val3 = new PdfPKCS7((ICipherParameters)null, (ICollection<X509Certificate>)chain, text, false);
				val3.SetExternalDigest(extSignature, (byte[])null, "RSA");
				TSAClientBouncyCastle val4 = null;
				if (timestampConfig.UseTimestamp)
				{
					val4 = new TSAClientBouncyCastle(timestampConfig.TsaUrl, timestampConfig.TsaAcc, timestampConfig.TsaPass);
				}
				byte[] encodedPKCS = val3.GetEncodedPKCS7(hash, signDate, (ITSAClient)(object)val4, (byte[])null, (ICollection<byte[]>)null, (CryptoStandard)0);
				int num = (int)(array[2] - array[1]) - 2;
				if ((num & 1) != 0)
				{
					LogSystem.Warn("Gap is not a multiple of 2");
					return false;
				}
				num /= 2;
				if (num < encodedPKCS.Length)
				{
					LogSystem.Warn("Not enough space");
					return false;
				}
				StreamUtil.CopyBytes(val2, 0L, array[1] + 1, outStream);
				ByteBuffer val5 = new ByteBuffer(num * 2);
				byte[] array2 = encodedPKCS;
				foreach (byte b in array2)
				{
					val5.AppendHex(b);
				}
				int num2 = (num - encodedPKCS.Length) * 2;
				for (int j = 0; j < num2; j++)
				{
					val5.Append((byte)48);
				}
				val5.WriteTo(outStream);
				StreamUtil.CopyBytes(val2, array[2] - 1, array[3] + 1, outStream);
				val2.Close();
				((Stream)(object)val5).Close();
				result = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn("Error insertSignature: " + ex.Message);
				result = false;
			}
			finally
			{
				if (val != null)
				{
					val.Close();
				}
			}
			return result;
		}

		public List<byte[]> PreSign(string inFile, string fieldName, X509Certificate[] chain, DisplayConfig displayConfig)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			PdfReader val = null;
			List<byte[]> result;
			try
			{
				List<byte[]> list = new List<byte[]>();
				val = new PdfReader(inFile);
				PdfDictionary signatureDictionary = val.AcroFields.GetSignatureDictionary(fieldName);
				if (signatureDictionary == null)
				{
					Console.WriteLine("No field");
					return null;
				}
				PdfArray asArray = signatureDictionary.GetAsArray(PdfName.BYTERANGE);
				long[] array = asArray.AsLongArray();
				if (asArray.Size != 4 || array[0] != 0)
				{
					Console.WriteLine("Single exclusion space supported");
					return null;
				}
				IRandomAccessSource val2 = val.SafeFile.CreateSourceView();
				PdfPKCS7 val3 = new PdfPKCS7((ICipherParameters)null, (ICollection<X509Certificate>)chain, (!string.IsNullOrEmpty(displayConfig.HashAlgorithm)) ? displayConfig.HashAlgorithm : HASH_ALG, false);
				byte[] array2 = DigestAlgorithms.Digest((Stream)new RASInputStream(new RandomAccessSourceFactory().CreateRanged(val2, (IList<long>)array)), DigestUtilities.GetDigest((!string.IsNullOrEmpty(displayConfig.HashAlgorithm)) ? displayConfig.HashAlgorithm : HASH_ALG));
				byte[] authenticatedAttributeBytes = val3.getAuthenticatedAttributeBytes(array2, displayConfig.SignDate, (byte[])null, (ICollection<byte[]>)null, (CryptoStandard)0);
				list.Add(authenticatedAttributeBytes);
				list.Add(array2);
				result = list;
			}
			catch (Exception ex)
			{
				LogSystem.Warn("Error create hash: " + ex.Message);
				result = null;
			}
			finally
			{
				try
				{
					if (val != null)
					{
						val.Close();
					}
				}
				catch (Exception ex2)
				{
					LogSystem.Warn("Error create hash: " + ex2.Message);
				}
			}
			return result;
		}
	}
}
