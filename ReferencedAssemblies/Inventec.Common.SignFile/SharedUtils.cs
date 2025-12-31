using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Inventec.Common.Logging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.X509;

namespace Inventec.Common.SignFile
{
	public class SharedUtils
	{
		internal class CertManager
		{
			private static X509Certificate2 _certificate;

			internal static X509Certificate2 Certificate
			{
				get
				{
					if (_certificate == null)
					{
						string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "3dcb620b-e826-436e-a018-6826b0b4ed7f.pfx");
						LogSystem.Info(text);
						if (!File.Exists(text))
						{
							throw new FileNotFoundException("Certificate file not found: " + text);
						}
						byte[] rawData = File.ReadAllBytes(text);
						_certificate = new X509Certificate2(rawData, "@123", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
					}
					return _certificate;
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass27_0
		{
			public float imgTotalWidth;

			public float plusH;

			public float widthImagePercent;
		}

		public static string ConvertTVKhongDau(string str)
		{
			string[] array = new string[17]
			{
				"à", "á", "ạ", "ả", "ã", "â", "ầ", "ấ", "ậ", "ẩ",
				"ẫ", "ă", "ằ", "ắ", "ặ", "ẳ", "ẵ"
			};
			string[] array2 = new string[17]
			{
				"À", "Á", "Ạ", "Ả", "Ã", "Â", "Ầ", "Ấ", "Ậ", "Ẩ",
				"Ẫ", "Ă", "Ằ", "Ắ", "Ặ", "Ẳ", "Ẵ"
			};
			string[] array3 = new string[11]
			{
				"è", "é", "ẹ", "ẻ", "ẽ", "ê", "ề", "ế", "ệ", "ể",
				"ễ"
			};
			string[] array4 = new string[11]
			{
				"È", "É", "Ẹ", "Ẻ", "Ẽ", "Ê", "Ề", "Ế", "Ệ", "Ể",
				"Ễ"
			};
			string[] array5 = new string[5] { "ì", "í", "ị", "ỉ", "ĩ" };
			string[] array6 = new string[5] { "Ì", "Í", "Ị", "Ỉ", "Ĩ" };
			string[] array7 = new string[17]
			{
				"ò", "ó", "ọ", "ỏ", "õ", "ô", "ồ", "ố", "ộ", "ổ",
				"ỗ", "ơ", "ờ", "ớ", "ợ", "ở", "ỡ"
			};
			string[] array8 = new string[17]
			{
				"Ò", "Ó", "Ọ", "Ỏ", "Õ", "Ô", "Ồ", "Ố", "Ộ", "Ổ",
				"Ỗ", "Ơ", "Ờ", "Ớ", "Ợ", "Ở", "Ỡ"
			};
			string[] array9 = new string[11]
			{
				"ù", "ú", "ụ", "ủ", "ũ", "ư", "ừ", "ứ", "ự", "ử",
				"ữ"
			};
			string[] array10 = new string[11]
			{
				"Ù", "Ú", "Ụ", "Ủ", "Ũ", "Ư", "Ừ", "Ứ", "Ự", "Ử",
				"Ữ"
			};
			string[] array11 = new string[5] { "ỳ", "ý", "ỵ", "ỷ", "ỹ" };
			string[] array12 = new string[5] { "Ỳ", "Ý", "Ỵ", "Ỷ", "Ỹ" };
			str = str.Replace("đ", "d");
			str = str.Replace("Đ", "D");
			string[] array13 = array;
			foreach (string oldValue in array13)
			{
				str = str.Replace(oldValue, "a");
			}
			string[] array14 = array2;
			foreach (string oldValue2 in array14)
			{
				str = str.Replace(oldValue2, "A");
			}
			string[] array15 = array3;
			foreach (string oldValue3 in array15)
			{
				str = str.Replace(oldValue3, "e");
			}
			string[] array16 = array4;
			foreach (string oldValue4 in array16)
			{
				str = str.Replace(oldValue4, "E");
			}
			string[] array17 = array5;
			foreach (string oldValue5 in array17)
			{
				str = str.Replace(oldValue5, "i");
			}
			string[] array18 = array6;
			foreach (string oldValue6 in array18)
			{
				str = str.Replace(oldValue6, "I");
			}
			string[] array19 = array7;
			foreach (string oldValue7 in array19)
			{
				str = str.Replace(oldValue7, "o");
			}
			string[] array20 = array8;
			foreach (string oldValue8 in array20)
			{
				str = str.Replace(oldValue8, "O");
			}
			string[] array21 = array9;
			foreach (string oldValue9 in array21)
			{
				str = str.Replace(oldValue9, "u");
			}
			string[] array22 = array10;
			foreach (string oldValue10 in array22)
			{
				str = str.Replace(oldValue10, "U");
			}
			string[] array23 = array11;
			foreach (string oldValue11 in array23)
			{
				str = str.Replace(oldValue11, "y");
			}
			string[] array24 = array12;
			foreach (string oldValue12 in array24)
			{
				str = str.Replace(oldValue12, "Y");
			}
			return str;
		}

		public static string GenerateTempFile()
		{
			try
			{
				string path = GenerateTempFolderWithin();
				return Path.Combine(path, Guid.NewGuid().ToString() + ".pdf");
			}
			catch (IOException ex)
			{
				Console.WriteLine("Error create temp file: " + ex.Message);
				return "";
			}
		}

		public static string GenerateTempFile(string ext)
		{
			try
			{
				string path = GenerateTempFolderWithin();
				return Path.Combine(path, Guid.NewGuid().ToString() + ((!string.IsNullOrEmpty(ext)) ? ext : ".pdf"));
			}
			catch (IOException ex)
			{
				Console.WriteLine("Error create temp file: " + ex.Message);
				return "";
			}
		}

		internal static string GenerateTempFolderWithin()
		{
			try
			{
				string text = GenerateTempFolderWithinByDate();
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
			catch (IOException ex)
			{
				LogSystem.Warn("Error create temp file: " + ex.Message);
				return "";
			}
		}

		internal static string GenerateTempFolderWithinByDate()
		{
			try
			{
				string text = Path.Combine(ParentTempFolder(), DateTime.Now.ToString("ddMMyyyy"));
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				try
				{
					string path = Path.Combine(ParentTempFolder(), DateTime.Now.AddDays(-1.0).ToString("ddMMyyyy"));
					if (Directory.Exists(path))
					{
						Directory.Delete(path, true);
					}
				}
				catch (Exception ex)
				{
					LogSystem.Warn("Error .Delete temp pre folder: " + ex.Message);
				}
				return text;
			}
			catch (IOException ex2)
			{
				LogSystem.Warn("Error create temp file: " + ex2.Message);
				return "";
			}
		}

		internal static string ParentTempFolder()
		{
			try
			{
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
			}
			catch (IOException ex)
			{
				LogSystem.Warn("Error create temp file: " + ex.Message);
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}

		public static void CreatePdf(PdfReader[] readers, ref string outFilePath)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			Document val = new Document();
			outFilePath = GenerateTempFile();
			PdfCopy val2 = new PdfCopy(val, (Stream)File.Open(outFilePath, FileMode.Create));
			val2.SetMergeFields();
			val.Open();
			foreach (PdfReader val3 in readers)
			{
				val2.AddDocument(val3);
			}
			val.Close();
			foreach (PdfReader val4 in readers)
			{
			}
		}

		public static void CreatePdf(string inFilePath, ref string outFilePath)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			Document val = new Document();
			outFilePath = GenerateTempFile();
			PdfCopy val2 = new PdfCopy(val, (Stream)File.Open(outFilePath, FileMode.Create));
			val2.SetMergeFields();
			val.Open();
			PdfReader val3 = new PdfReader(inFilePath);
			val2.AddDocument(val3);
			val.Close();
			val3.Close();
		}

		public static bool SaveNewFileFromReader(PdfReader reader, ref string outFilePath, bool isCloseReader)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			bool result = false;
			try
			{
				outFilePath = GenerateTempFile();
				using (FileStream fileStream = File.Open(outFilePath, FileMode.Create))
				{
					PdfReader val = new PdfReader(reader);
					PdfConcatenate val2 = new PdfConcatenate((Stream)fileStream);
					List<int> list = new List<int>();
					for (int i = 0; i <= val.NumberOfPages; i++)
					{
						list.Add(i);
					}
					val.SelectPages((ICollection<int>)list);
					val2.AddPages(val);
					val.Close();
					val2.Close();
				}
				result = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error insertSignature: " + ex.Message);
				result = false;
			}
			finally
			{
				if (isCloseReader && reader != null)
				{
					reader.Close();
				}
			}
			return result;
		}

		public static bool SaveNewFileFromReader(string filename, ref string outFilePath)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			bool result = false;
			try
			{
				outFilePath = GenerateTempFile();
				PdfReader val = new PdfReader(filename);
				List<int> list = new List<int>();
				for (int i = 0; i <= val.NumberOfPages; i++)
				{
					list.Add(i);
				}
				val.SelectPages((ICollection<int>)list);
				PdfStamper val2 = new PdfStamper(val, (Stream)new FileStream(outFilePath, FileMode.Create));
				val2.Close();
				val.Close();
				result = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error insertSignature: " + ex.Message);
				result = false;
			}
			finally
			{
			}
			return result;
		}

		public static bool SaveNewFileFromReader(byte[] bfile, ref string outFilePath, string ext = ".pdf")
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			bool result = false;
			try
			{
				outFilePath = GenerateTempFile(ext);
				if (ext == ".pdf")
				{
					using (FileStream fileStream = File.Open(outFilePath, FileMode.Create, FileAccess.ReadWrite))
					{
						PdfConcatenate val = new PdfConcatenate((Stream)fileStream);
						PdfReader val2 = new PdfReader(bfile);
						List<int> list = new List<int>();
						for (int i = 0; i <= val2.NumberOfPages; i++)
						{
							list.Add(i);
						}
						val2.SelectPages((ICollection<int>)list);
						val.AddPages(val2);
						val2.Close();
						val.Close();
					}
				}
				else
				{
					ByteToFile(bfile, outFilePath);
				}
				result = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error insertSignature: " + ex.Message);
				result = false;
			}
			finally
			{
			}
			return result;
		}

		public static bool SaveNewFileFromReaderExt(byte[] bfile, ref string outFilePath, string ext = ".pdf")
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			bool result = false;
			try
			{
				outFilePath = GenerateTempFile(ext);
				if (ext == ".pdf")
				{
					using (FileStream fileStream = File.Open(outFilePath, FileMode.Create, FileAccess.ReadWrite))
					{
						PdfConcatenate val = new PdfConcatenate((Stream)fileStream);
						PdfReader val2 = new PdfReader(bfile);
						List<int> list = new List<int>();
						for (int i = 0; i <= val2.NumberOfPages; i++)
						{
							list.Add(i);
						}
						val2.SelectPages((ICollection<int>)list);
						val.AddPages(val2);
						val2.Close();
						val.Close();
					}
				}
				else
				{
					ByteToFile(bfile, outFilePath);
				}
				result = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error insertSignature: " + ex.Message);
				result = false;
			}
			finally
			{
			}
			return result;
		}

		public static bool SaveNewFileFromReader(Stream sfile, ref string outFilePath)
		{
			bool result = false;
			try
			{
				outFilePath = GenerateTempFile();
				ByteToFile(StreamToByte(sfile), outFilePath);
				result = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error SaveNewFileFromReader: " + ex.Message);
				try
				{
					File.Delete(outFilePath);
				}
				catch
				{
				}
				result = false;
			}
			finally
			{
			}
			return result;
		}

		public static byte[] GetBytes(string str)
		{
			try
			{
				byte[] array = new byte[str.Length * 2];
				Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
				return array;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error get Byte from string: " + ex.Message);
				return null;
			}
		}

        public static string GetCN(Org.BouncyCastle.X509.X509Certificate cert)
		{
			try
			{
				return GetCNFromDN(GetSubject(cert));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error getCN: " + ex.Message);
				return null;
			}
		}

		public static string GetCNFromDN(string dn)
		{
			try
			{
				char[] separator = new char[1] { ',' };
				string[] array = dn.Split(separator);
				string result = "";
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].IndexOf("CN=") != -1)
					{
						char[] separator2 = new char[1] { '=' };
						result = array[i].Split(separator2)[1];
					}
				}
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error GetCNFromDN: " + ex.Message);
				return "";
			}
		}

		public static double GetCurrentMilli()
		{
			try
			{
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				return (long)(DateTime.UtcNow - dateTime).TotalMilliseconds;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error get current milli: " + ex.Message);
				return 0.0;
			}
		}

        public static string GetLocation(Org.BouncyCastle.X509.X509Certificate certificate)
		{
			try
			{
				return GetLocationFromDN(GetSubject(certificate));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error getLocation: " + ex.Message);
				return null;
			}
		}

		public static string GetLocationFromDN(string dn)
		{
			try
			{
				char[] separator = new char[1] { ',' };
				string[] array = dn.Split(separator);
				string text = "";
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].IndexOf("L=") != -1)
					{
						char[] separator2 = new char[1] { '=' };
						text = array[i].Split(separator2)[1];
					}
				}
				if (text != "")
				{
					return ConvertTVKhongDau(text);
				}
				return text;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error GetLocationFromDN: " + ex.Message);
				return "";
			}
		}

		public static string getSignName()
		{
			return GetCurrentMilli().ToString();
		}

        public static string GetSubject(Org.BouncyCastle.X509.X509Certificate certificate)
		{
			try
			{
				return ((object)certificate.SubjectDN).ToString();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error getSubject: " + ex.Message);
				return null;
			}
		}

		public static BaseFont GetBaseFont()
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.ttf");
			return BaseFont.CreateFont(text, "Identity-H", false);
		}

		internal static SecureString GetSecurePin(string PinCode)
		{
			SecureString secureString = new SecureString();
			char[] array = PinCode.ToCharArray();
			foreach (char c in array)
			{
				secureString.AppendChar(c);
			}
			return secureString;
		}

		internal static string GetFileContentHash(string fileContent)
		{
			try
			{
				using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create())
				{
					byte[] array = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(fileContent));
					string text = BitConverter.ToString(array);
					return text.Replace("-", string.Empty);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return "";
		}

		internal static byte[] StreamToByte(Stream input)
		{
			byte[] array = new byte[16384];
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int count;
				while ((count = input.Read(array, 0, array.Length)) > 0)
				{
					memoryStream.Write(array, 0, count);
				}
				return memoryStream.ToArray();
			}
		}

		internal static byte[] FileToByte(string input)
		{
			return File.ReadAllBytes(input);
		}

		internal static void ByteToFile(byte[] arrInFile, string saveFile)
		{
			File.WriteAllBytes(saveFile, arrInFile);
		}

		internal static void UpdateimageCell(float widthRectangle, float heightRectangle, Image instance, float SignaltureImageWidth, float widthImagePercent, float plusH)
		{
			_003C_003Ec__DisplayClass27_0 CS_0024_003C_003E8__locals26 = new _003C_003Ec__DisplayClass27_0();
			CS_0024_003C_003E8__locals26.plusH = plusH;
			CS_0024_003C_003E8__locals26.widthImagePercent = widthImagePercent;
			CS_0024_003C_003E8__locals26.imgTotalWidth = 0f;
			if (instance != null)
			{
				float num = heightRectangle - CS_0024_003C_003E8__locals26.plusH;
				float num2 = ((num > 0f) ? num : heightRectangle);
				if (((Rectangle)instance).Width > widthRectangle || ((Rectangle)instance).Height > heightRectangle || (((Rectangle)instance).Height > num && num > 0f))
				{
					float weightImgRealPercentTH1 = ((((Rectangle)instance).Width > widthRectangle) ? (widthRectangle / ((Rectangle)instance).Width) : 0f);
					float heightImgRealPercentTH1 = ((((Rectangle)instance).Height > num2) ? (num2 / ((Rectangle)instance).Height) : 0f);
					if (weightImgRealPercentTH1 > 0f && heightImgRealPercentTH1 > 0f)
					{
						CS_0024_003C_003E8__locals26.imgTotalWidth = ((Rectangle)instance).Width * CS_0024_003C_003E8__locals26.widthImagePercent * ((weightImgRealPercentTH1 < heightImgRealPercentTH1) ? weightImgRealPercentTH1 : heightImgRealPercentTH1) / 100f;
					}
					else if (heightImgRealPercentTH1 > 0f)
					{
						CS_0024_003C_003E8__locals26.imgTotalWidth = ((Rectangle)instance).Width * CS_0024_003C_003E8__locals26.widthImagePercent * heightImgRealPercentTH1 / 100f;
					}
					else if (weightImgRealPercentTH1 > 0f)
					{
						CS_0024_003C_003E8__locals26.imgTotalWidth = ((Rectangle)instance).Width * CS_0024_003C_003E8__locals26.widthImagePercent * weightImgRealPercentTH1 / 100f;
					}
					LogSystem.Info("2__" + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => weightImgRealPercentTH1)), (object)weightImgRealPercentTH1) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => heightImgRealPercentTH1)), (object)heightImgRealPercentTH1) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => CS_0024_003C_003E8__locals26.imgTotalWidth)), (object)CS_0024_003C_003E8__locals26.imgTotalWidth) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => CS_0024_003C_003E8__locals26.plusH)), (object)CS_0024_003C_003E8__locals26.plusH));
				}
				else
				{
					float newHeightImagePercent = ((num > 0f) ? num : heightRectangle) / ((Rectangle)instance).Height;
					CS_0024_003C_003E8__locals26.imgTotalWidth = ((Rectangle)instance).Width * CS_0024_003C_003E8__locals26.widthImagePercent * newHeightImagePercent / 100f;
                    //LogSystem.Info("3__" + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => newHeightImagePercent)), (object)newHeightImagePercent) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => CS_0024_003C_003E8__locals26.imgTotalWidth)), (object)CS_0024_003C_003E8__locals26.imgTotalWidth) + LogUtil.TraceData(LogUtil.GetMemberName<float>(Expression.Lambda<Func<float>>(Expression.Field(Expression.Constant(CS_0024_003C_003E8__locals26, typeof(_003C_003Ec__DisplayClass27_0)), FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[0])), (object)CS_0024_003C_003E8__locals26.plusH));
				}
			}
			else
			{
				CS_0024_003C_003E8__locals26.imgTotalWidth = widthRectangle * CS_0024_003C_003E8__locals26.widthImagePercent / 100f;
                //LogSystem.Info("4__" + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => CS_0024_003C_003E8__locals26.imgTotalWidth)), (object)CS_0024_003C_003E8__locals26.imgTotalWidth) + LogUtil.TraceData(LogUtil.GetMemberName<float>(Expression.Lambda<Func<float>>(Expression.Field(Expression.Constant(CS_0024_003C_003E8__locals26, typeof(_003C_003Ec__DisplayClass27_0)), FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[0])), (object)CS_0024_003C_003E8__locals26.widthImagePercent));
			}
		}

		public static float CalculateWidthPercent(float widthRectangle, float heightRectangle, Image instance, float SignaltureImageWidth, float widthImagePercent, float plusH)
		{
			float imgTotalWidth = ((Rectangle)instance).Width;
			if (instance != null)
			{
				float heightRecModPlus = heightRectangle - plusH;
				float num = ((heightRecModPlus > 0f) ? heightRecModPlus : heightRectangle);
				if (((Rectangle)instance).Width > widthRectangle || ((Rectangle)instance).Height > heightRectangle || (((Rectangle)instance).Height > heightRecModPlus && heightRecModPlus > 0f))
				{
					float weightImgRealPercentTH1 = ((((Rectangle)instance).Width > widthRectangle) ? (widthRectangle / ((Rectangle)instance).Width) : 0f);
					float heightImgRealPercentTH1 = ((((Rectangle)instance).Height > num) ? (num / ((Rectangle)instance).Height) : 0f);
					if (weightImgRealPercentTH1 > 0f && heightImgRealPercentTH1 > 0f)
					{
						imgTotalWidth = ((Rectangle)instance).Width * widthImagePercent * ((weightImgRealPercentTH1 < heightImgRealPercentTH1) ? weightImgRealPercentTH1 : heightImgRealPercentTH1) / 100f;
					}
					else if (heightImgRealPercentTH1 > 0f)
					{
						imgTotalWidth = ((Rectangle)instance).Width * widthImagePercent * heightImgRealPercentTH1 / 100f;
					}
					else if (weightImgRealPercentTH1 > 0f)
					{
						imgTotalWidth = ((Rectangle)instance).Width * widthImagePercent * weightImgRealPercentTH1 / 100f;
					}
					LogSystem.Info("2__" + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => weightImgRealPercentTH1)), (object)weightImgRealPercentTH1) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => heightImgRealPercentTH1)), (object)heightImgRealPercentTH1) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => widthRectangle)), (object)widthRectangle) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => heightRectangle)), (object)heightRectangle) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => imgTotalWidth)), (object)imgTotalWidth) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => heightRecModPlus)), (object)heightRecModPlus) + LogUtil.TraceData("instance.Height", (object)((Rectangle)instance).Height) + LogUtil.TraceData("instance.Width", (object)((Rectangle)instance).Width) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => plusH)), (object)plusH));
				}
				else
				{
					float newHeightImagePercent = ((heightRecModPlus > 0f) ? heightRecModPlus : heightRectangle) / ((Rectangle)instance).Height;
					imgTotalWidth = ((Rectangle)instance).Width * widthImagePercent * newHeightImagePercent / 100f;
					LogSystem.Info("3__" + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => newHeightImagePercent)), (object)newHeightImagePercent) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => widthRectangle)), (object)widthRectangle) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => heightRectangle)), (object)heightRectangle) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => imgTotalWidth)), (object)imgTotalWidth) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => heightRecModPlus)), (object)heightRecModPlus) + LogUtil.TraceData("instance.Height", (object)((Rectangle)instance).Height) + LogUtil.TraceData("instance.Width", (object)((Rectangle)instance).Width) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => plusH)), (object)plusH));
				}
			}
			else
			{
				imgTotalWidth = widthRectangle * widthImagePercent / 100f;
				LogSystem.Info("4__" + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => widthRectangle)), (object)widthRectangle) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => heightRectangle)), (object)heightRectangle) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => imgTotalWidth)), (object)imgTotalWidth) + LogUtil.TraceData("instance.Height", (object)((Rectangle)instance).Height) + LogUtil.TraceData("instance.Width", (object)((Rectangle)instance).Width) + LogUtil.TraceData(LogUtil.GetMemberName<float>((Expression<Func<float>>)(() => widthImagePercent)), (object)widthImagePercent));
			}
			float num2 = widthRectangle;
			float num3 = ((imgTotalWidth > ((Rectangle)instance).Width) ? ((Rectangle)instance).Width : imgTotalWidth);
			if (SignaltureImageWidth > 0f)
			{
				num2 = SignaltureImageWidth;
			}
			else if (widthRectangle >= 100f)
			{
				num2 = 100f;
				if (num3 < 140f)
				{
					float num4 = 0f;
					num4 = num2;
					num2 = num3;
					num3 = num4 * 2f;
				}
			}
			else
			{
				num2 = widthRectangle;
				if (num3 < widthRectangle)
				{
					float num5 = 0f;
					num5 = num2;
					num2 = num3;
					num3 = num5;
				}
			}
			return 100f * (num2 / num3);
		}
	}
}
