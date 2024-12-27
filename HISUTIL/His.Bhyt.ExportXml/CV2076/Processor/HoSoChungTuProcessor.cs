using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.CV2076.ADO;
using His.Bhyt.ExportXml.CV2076.HoSo;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.CV2076.Processor
{
    class HoSoChungTuProcessor : XmlProcessorBase
    {
        InputADO Data { get; set; }
        internal HoSoChungTuProcessor(InputADO data)
        {
            this.Data = data;
        }

        internal bool Processor()
        {
            bool result = false;
            try
            {
                ResultADO hoSoChungTu = this.CreateHoSoChungTu();
                if (hoSoChungTu == null || !hoSoChungTu.Success || hoSoChungTu.Data == null || hoSoChungTu.Data.Length == 0)
                    return false;
                var fileName = string.Format("XML2076_{0}___{1}", new string[] { DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss"), Data.Treatment2076.TREATMENT_CODE });
                var path = string.Format("{0}/{1}.xml", GlobalConfigStore.PathSaveXml, fileName);
                var rs = this.CreatedXmlFile(hoSoChungTu.Data[0] as HoSoChungTu, false, true, true, path);
                if (rs == null || !rs.Success)
                    return false;
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal MemoryStream ProcessorPlus()
        {
            MemoryStream result = null;
            try
            {
                ResultADO hoSoChungTu = this.CreateHoSoChungTu();
                if (hoSoChungTu == null || !hoSoChungTu.Success || hoSoChungTu.Data == null || hoSoChungTu.Data.Length == 0)
                    return null;
                var fileName = string.Format("XML2076_{0}___{1}", new string[] { DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss"), Data.Treatment2076.TREATMENT_CODE });
                var path = string.Format("{0}/{1}.xml", GlobalConfigStore.PathSaveXml, fileName);
                result = this.CreatedXmlFileEncodingPlus(hoSoChungTu.Data[0] as HoSoChungTu, false, true, true, path);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal string ProcessorPath()
        {
            string result = "";
            try
            {
                ResultADO hoSoChungTu = this.CreateHoSoChungTu();
                if (hoSoChungTu == null || !hoSoChungTu.Success || hoSoChungTu.Data == null || hoSoChungTu.Data.Length == 0)
                    return "";
                var fileName = string.Format("XML2076_{0}___{1}", new string[] { DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss"), Data.Treatment2076.TREATMENT_CODE });
                var path = string.Format("{0}/{1}.xml", GlobalConfigStore.PathSaveXml, fileName);
                var rs = this.CreatedXmlFileEncoding(hoSoChungTu.Data[0] as HoSoChungTu, false, true, true, path);
                if (rs == null || !rs.Success)
                    return "";
                result = path;
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private ResultADO CreateHoSoChungTu()
        {
            ResultADO rs = null;
            try
            {
                ResultADO ttDonVi = this.CreateThongTinDonVi();
                if (ttDonVi == null || !ttDonVi.Success || ttDonVi.Data == null || ttDonVi.Data.Length == 0)
                    return rs = new ResultADO(false, "Tao Thong Tin Don Vi That Bai", null);
                ResultADO ttHoSo = this.CreateThongTinHoSo();
                if (ttHoSo == null || !ttHoSo.Success || ttHoSo.Data == null || ttHoSo.Data.Length == 0)
                    return rs = new ResultADO(false, "Tao Thong Tin Ho So That Bai", null);
                HoSoChungTu giamdinh = new HoSoChungTu();
                giamdinh.THONGTINDONVI = ttDonVi.Data[0] as ThongTinDonVi;
                giamdinh.THONGTINHOSO = ttHoSo.Data[0] as ThongTinHoSo;
                rs = new ResultADO(true, "", new object[] { giamdinh });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateThongTinDonVi()
        {
            ResultADO rs = null;
            try
            {
                ThongTinDonVi ttDonVi = new ThongTinDonVi();
                if (Data.Branch != null)
                {
                    ttDonVi.MACSKCB = Data.Branch.HEIN_MEDI_ORG_CODE;
                    ttDonVi.CHUKYDONVI = GlobalConfigStore.Signature ?? "";
                }
                else
                {
                    ttDonVi.MACSKCB = GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE;
                    ttDonVi.CHUKYDONVI = GlobalConfigStore.Signature ?? "";
                }
                rs = new ResultADO(true, "", new object[] { ttDonVi });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateThongTinHoSo()
        {
            ResultADO rs = null;
            try
            {
                var dsHoso = this.CreateDanhSachHoSo();
                if (dsHoso == null || !dsHoso.Success || dsHoso.Data == null || dsHoso.Data.Length == 0)
                    return rs = new ResultADO(false, "Tao Danh Sach Ho So That Bai", null);
                ThongTinHoSo thongtinHoSo = new ThongTinHoSo();
                thongtinHoSo.NGAYLAP = DateTime.Now.ToString("yyyyMMdd");
                thongtinHoSo.DANHSACHHOSO = dsHoso.Data[0] as DanhSachHoSo;
                thongtinHoSo.SOLUONGHOSO = thongtinHoSo.DANHSACHHOSO.HOSO.Count;
                rs = new ResultADO(true, "", new object[] { thongtinHoSo });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateDanhSachHoSo()
        {
            ResultADO rs = null;
            try
            {
                var hoSo = this.CreateHoSo();
                if (hoSo == null || !hoSo.Success || hoSo.Data == null || hoSo.Data.Length == 0)
                    throw new Exception("Tao FileHoSo that bai: " + LogUtil.TraceData("hoSo", hoSo));
                DanhSachHoSo dsHoSo = new DanhSachHoSo();
                dsHoSo.HOSO = hoSo.Data[0] as List<HoSo.HoSo>;
                rs = new ResultADO(true, "", new object[] { dsHoSo });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateHoSo()
        {
            ResultADO rs = null;
            try
            {
                var fileHoSo = this.CreateFileHoSo();
                if (fileHoSo == null || !fileHoSo.Success || fileHoSo.Data == null || fileHoSo.Data.Length == 0)
                    throw new Exception("Tao FileHoSo that bai: " + LogUtil.TraceData("fileHoSo", fileHoSo));
                List<FileHoSo> lstFile = fileHoSo.Data[0] as List<FileHoSo>;
                List<HoSo.HoSo> lstHoSo = new List<HoSo.HoSo>();
                foreach (FileHoSo fHoSo in lstFile)
                {
                    HoSo.HoSo hs = new HoSo.HoSo();
                    hs.FILEHOSO = fHoSo;
                    lstHoSo.Add(hs);
                }
                rs = new ResultADO(true, "", new object[] { lstHoSo });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateFileHoSo()
        {
            ResultADO rs = null;
            try
            {
                var file = this.CreateNoiDungFile();
                if (file == null || !file.Success || file.Data == null || file.Data.Length < 5)
                    return rs = new ResultADO(false, "File ho so khong du 5 file", null);
                string giayRaVien = file.Data[0] as string;
                string hoSoBenhAn = file.Data[1] as string;
                List<string> dsGiayChungSinh = file.Data[2] as List<string>;
                string nghiDuongThai = file.Data[3] as string;
                string nghiOm = file.Data[4] as string;

                List<FileHoSo> lstFileHoSo = new List<FileHoSo>();
                if (nghiOm != null)
                {
                    FileHoSo fHoSo = new FileHoSo();
                    fHoSo.LOAIHOSO = "CT07";
                    fHoSo.NOIDUNGFILE = EncodeBase64(Encoding.UTF8, nghiOm); ;
                    lstFileHoSo.Add(fHoSo);
                }

                if (dsGiayChungSinh != null && dsGiayChungSinh.Count > 0)
                {
                    foreach (string xmldata in dsGiayChungSinh)
                    {
                        FileHoSo fHoSo = new FileHoSo();
                        fHoSo.LOAIHOSO = "CT05";
                        fHoSo.NOIDUNGFILE = EncodeBase64(Encoding.UTF8, xmldata);
                        lstFileHoSo.Add(fHoSo);
                    }
                }

                if (giayRaVien != null)
                {
                    FileHoSo fHoSo = new FileHoSo();
                    fHoSo.LOAIHOSO = "CT03";
                    fHoSo.NOIDUNGFILE = EncodeBase64(Encoding.UTF8, giayRaVien);
                    lstFileHoSo.Add(fHoSo);
                }

                if (hoSoBenhAn != null)
                {
                    FileHoSo fHoSo = new FileHoSo();
                    fHoSo.LOAIHOSO = "CT04";
                    fHoSo.NOIDUNGFILE = EncodeBase64(Encoding.UTF8, hoSoBenhAn);
                    lstFileHoSo.Add(fHoSo);
                }

                if (nghiDuongThai != null)
                {
                    FileHoSo fHoSo = new FileHoSo();
                    fHoSo.LOAIHOSO = "CT06";
                    fHoSo.NOIDUNGFILE = EncodeBase64(Encoding.UTF8, nghiDuongThai); ;
                    lstFileHoSo.Add(fHoSo);
                }

                rs = new ResultADO(true, "", new object[] { lstFileHoSo });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateNoiDungFile()
        {
            ResultADO rs = null;
            try
            {
                string xmlCt03Data = null;
                string xmlCt04Data = null;
                List<string> listXmlCt05Data = null;
                string xmlCt06Data = null;
                string xmlCt07Data = null;

                //Xử lý xml ct03
                XmlCt03ADO giayRaVienAdo = null;
                XmlCt03Processor ct03Processor = new XmlCt03Processor();
                if (string.IsNullOrWhiteSpace(this.Data.XML2076__DOC_CODE) || this.Data.XML2076__DOC_CODE == "CT03")
                {
                    ResultADO ct03Result = ct03Processor.GenerateXmlCt03ADO(this.Data);
                    if (ct03Result == null || !ct03Result.Success || ct03Result.Data == null)
                        return ct03Result;
                    giayRaVienAdo = ct03Result.Data[0] as XmlCt03ADO;
                }

                //Xử lý xml ct04
                XmlCt04ADO hosobenhanAdo = null;
                XmlCt04Processor ct04Processor = new XmlCt04Processor();
                if (string.IsNullOrWhiteSpace(this.Data.XML2076__DOC_CODE) || this.Data.XML2076__DOC_CODE == "CT04")
                {
                    ResultADO ct04Result = ct04Processor.GenerateXmlCt04ADO(this.Data);
                    if (ct04Result == null || !ct04Result.Success || ct04Result.Data == null)
                        return ct04Result;
                    hosobenhanAdo = ct04Result.Data[0] as XmlCt04ADO;
                }

                //xu ly xml ct05
                List<XmlCt05ADO> giayChungSinhAdo = null;
                XmlCt05Processor ct05Processor = new XmlCt05Processor();
                if (string.IsNullOrWhiteSpace(this.Data.XML2076__DOC_CODE) || this.Data.XML2076__DOC_CODE == "CT05")
                {
                    ResultADO ct05Result = ct05Processor.GenerateXmlCt05ADO(this.Data);
                    if (ct05Result == null || !ct05Result.Success || ct05Result.Data == null)
                        return ct05Result;
                    giayChungSinhAdo = ct05Result.Data[0] as List<XmlCt05ADO>;
                }

                //xu ly xml ct06
                XmlCt06ADO nghiDuongThaiAdo = null;
                XmlCt06Processor ct06Processor = new XmlCt06Processor();
                if (string.IsNullOrWhiteSpace(this.Data.XML2076__DOC_CODE) || this.Data.XML2076__DOC_CODE == "CT06")
                {
                    ResultADO ct06Result = ct06Processor.GenerateXmlCt06ADO(this.Data);
                    if (ct06Result == null || !ct06Result.Success || ct06Result.Data == null)
                        return ct06Result;
                    nghiDuongThaiAdo = ct06Result.Data[0] as XmlCt06ADO;
                }

                //Xu ly xml ct07
                XmlCt07ADO nghiOmAdo = null;
                XmlCt07Processor ct07Processor = new XmlCt07Processor();
                if (string.IsNullOrWhiteSpace(this.Data.XML2076__DOC_CODE) || this.Data.XML2076__DOC_CODE == "CT07")
                {
                    ResultADO ct07Result = ct07Processor.GenerateXmlCt07ADO(this.Data);
                    if (ct07Result == null || !ct07Result.Success || ct07Result.Data == null)
                        return ct07Result;
                    nghiOmAdo = ct07Result.Data[0] as XmlCt07ADO;
                }

                //process map to Data
                if (giayRaVienAdo != null)
                    ct03Processor.MapADOToXml(giayRaVienAdo, ref xmlCt03Data);
                if (hosobenhanAdo != null)
                    ct04Processor.MapADOToXml(hosobenhanAdo, ref xmlCt04Data);
                if (giayChungSinhAdo != null && giayChungSinhAdo.Count > 0)
                    ct05Processor.MapADOToXml(giayChungSinhAdo, ref listXmlCt05Data);
                if (nghiDuongThaiAdo != null)
                    ct06Processor.MapADOToXml(nghiDuongThaiAdo, ref xmlCt06Data);
                if (nghiOmAdo != null)
                    ct07Processor.MapADOToXml(nghiOmAdo, ref xmlCt07Data);


                if (xmlCt03Data == null && xmlCt04Data == null && listXmlCt05Data == null && xmlCt06Data == null && xmlCt07Data == null)
                    return rs = new ResultADO(false, "Khong Map duoc tu ADO sang XMLDetail", null);

                rs = new ResultADO();
                rs.Success = true;
                rs.Data = new object[] { xmlCt03Data, xmlCt04Data, listXmlCt05Data, xmlCt06Data, xmlCt07Data };
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return rs;
        }
    }
}
