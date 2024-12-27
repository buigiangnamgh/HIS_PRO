using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.QD917.ADO;
using His.Bhyt.ExportXml.QD917.XML.HoSo;
using His.Bhyt.ExportXml.QD917.XML.XML1;
using His.Bhyt.ExportXml.QD917.XML.XML2;
using His.Bhyt.ExportXml.QD917.XML.XML3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.QD917.Processor
{
    internal class HoSoProcessor : XmlProcessorBase
    {
        InputADO Data { get; set; }
        internal HoSoProcessor(InputADO data)
        {
            this.Data = data;
        }

        internal bool Processor()
        {
            bool result = false;
            try
            {
                var giamdinh = this.CreateGiamDinhHoSo();
                if (giamdinh == null || !giamdinh.Success || giamdinh.Data == null || giamdinh.Data.Length == 0)
                    return false;
                var fileName = string.Format("{0}___{1}_{2}_{3}", new string[] { DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss"), Data.HeinApproval.HEIN_CARD_NUMBER, Data.HeinApproval.TREATMENT_CODE, Data.HeinApproval.HEIN_APPROVAL_CODE });
                var path = string.Format("{0}/{1}.xml", GlobalConfigStore.PathSaveXml, fileName);
                var rs = this.CreatedXmlFile(giamdinh.Data[0] as GiamDinhHoSo, false, true, true, path);
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
                var giamdinh = this.CreateGiamDinhHoSo();
                if (giamdinh == null || !giamdinh.Success || giamdinh.Data == null || giamdinh.Data.Length == 0)
                    return null;
                var fileName = string.Format("{0}___{1}_{2}_{3}", new string[] { DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss"), Data.HeinApproval.HEIN_CARD_NUMBER, Data.HeinApproval.TREATMENT_CODE, Data.HeinApproval.HEIN_APPROVAL_CODE });
                var path = string.Format("{0}/{1}.xml", GlobalConfigStore.PathSaveXml, fileName);
                result = this.CreatedXmlFilePlus(giamdinh.Data[0] as GiamDinhHoSo, false, true, true, path);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private ResultADO CreateGiamDinhHoSo()
        {
            ResultADO rs = null;
            try
            {
                var ttDonVi = this.CreateThongTinDonVi();
                if (ttDonVi == null || !ttDonVi.Success || ttDonVi.Data == null || ttDonVi.Data.Length == 0)
                    return rs = new ResultADO(false, "Tao Thong Tin Don Vi That Bai", null);
                var ttHoSo = this.CreateThongTinHoSo();
                if (ttHoSo == null || !ttHoSo.Success || ttHoSo.Data == null || ttHoSo.Data.Length == 0)
                    return rs = new ResultADO(false, "Tao Thong Tin Ho So That Bai", null);
                GiamDinhHoSo giamdinh = new GiamDinhHoSo();
                giamdinh.CHUKYDONVI = GlobalConfigStore.Signature ?? "";
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
                }
                else
                {
                    ttDonVi.MACSKCB = GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE;
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
                thongtinHoSo.SOLUONGHOSO = 1;
                thongtinHoSo.DANHSACHHOSO = dsHoso.Data[0] as DanhSachHoSo;
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
                    throw new Exception("Tao FileHoSo that bai: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => hoSo), hoSo));
                DanhSachHoSo dsHoSo = new DanhSachHoSo();
                dsHoSo.HOSO = hoSo.Data[0] as HoSo;
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
                    throw new Exception("Tao FileHoSo that bai: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => fileHoSo), fileHoSo));
                HoSo hoSo = new HoSo();
                hoSo.FILEHOSO = fileHoSo.Data[0] as List<FileHoSo>;
                rs = new ResultADO(true, "", new object[] { hoSo });
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
                if (file == null || !file.Success || file.Data == null || file.Data.Length < 3)
                    return rs = new ResultADO(false, "File ho so khong du 3 file", null);
                XML1Data dataXml1 = file.Data[0] as XML1Data;
                XML2ListDetailData dsChiTietThuoc = file.Data[1] as XML2ListDetailData;
                XML3ListDetailData dsChiTietDVKT = file.Data[2] as XML3ListDetailData;
                var listFileXml = new List<FileHoSo>();
                //Create noi dung fileHoSo
                var createXml1File = this.CreatedXmlFile(dataXml1, true, false, false, string.Empty);
                if (createXml1File == null || !createXml1File.Success || createXml1File.Data == null || createXml1File.Data.Length == 0)
                    return new ResultADO(false, "Khong tao duoc file Xml 1", null);

                FileHoSo fileXml1 = new FileHoSo();
                fileXml1.LOAIHOSO = XmlType.XML1;
                fileXml1.NOIDUNGFILE = createXml1File.Data[0] as string;
                listFileXml.Add(fileXml1);

                if (dsChiTietThuoc.CHI_TIET_THUOC != null && dsChiTietThuoc.CHI_TIET_THUOC.Count > 0)
                {
                    var createXml2File = this.CreatedXmlFile(dsChiTietThuoc, true, false, false, string.Empty);
                    if (createXml2File == null || !createXml2File.Success || createXml2File.Data == null || createXml2File.Data.Length == 0)
                        return new ResultADO(false, "Khong tao duoc file Xml 2", null);
                    FileHoSo fileXml2 = new FileHoSo();
                    fileXml2.LOAIHOSO = XmlType.XML2;
                    fileXml2.NOIDUNGFILE = createXml2File.Data[0] as string;
                    listFileXml.Add(fileXml2);
                }

                if (dsChiTietDVKT.CHI_TIET_DVKT != null && dsChiTietDVKT.CHI_TIET_DVKT.Count > 0)
                {
                    var createXml3File = this.CreatedXmlFile(dsChiTietDVKT, true, false, false, string.Empty);
                    if (createXml3File == null || !createXml3File.Success || createXml3File.Data == null || createXml3File.Data.Length == 0)
                        return new ResultADO(false, "Khong tao duoc file Xml 3", null);
                    FileHoSo fileXml3 = new FileHoSo();
                    fileXml3.LOAIHOSO = XmlType.XML3;
                    fileXml3.NOIDUNGFILE = createXml3File.Data[0] as string;
                    listFileXml.Add(fileXml3);
                }
                rs = new ResultADO(true, "", new object[] { listFileXml });
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
                XML1Data Xml1Data = null;
                List<XML2DetailData> listDetailXml2 = new List<XML2DetailData>();
                List<XML3DetailData> listDetailXml3 = new List<XML3DetailData>();
                //Xử lý xml2
                Xml2Processor xml2Processor = new Xml2Processor();
                var xml2Result = xml2Processor.GenerateXml2ADO(this.Data);
                if (xml2Result == null || !xml2Result.Success || xml2Result.Data == null || xml2Result.Data.Length == 0)
                    return xml2Result;
                var listXmlThuocAdo = xml2Result.Data[0] as List<Xml2ADO>;

                //Xử lý xml 3
                Xml3Processor xml3Processor = new Xml3Processor();
                var xml3Result = xml3Processor.GenerateXml2ADO(this.Data);
                if (xml3Result == null || !xml3Result.Success || xml3Result.Data == null || xml3Result.Data.Length == 0)
                    return xml3Result;
                var listXmlDvktVTAdo = xml3Result.Data[0] as List<Xml3ADO>;

                //Xu ly Xml 1
                Xml1Processor xml1Procssor = new Xml1Processor();
                var xml1Result = xml1Procssor.GenerateXml1Data(this.Data, listXmlThuocAdo, listXmlDvktVTAdo);
                if (xml1Result == null || !xml1Result.Success || xml1Result.Data == null || xml1Result.Data.Length == 0)
                    return xml1Result;
                Xml1Data = xml1Result.Data[0] as XML1Data;

                //process listXml2Detail, listXml3Detail
                xml2Processor.MapADOToXml(listXmlThuocAdo, ref listDetailXml2);
                xml3Processor.MapADOToXml(listXmlDvktVTAdo, ref listDetailXml3);

                if (Xml1Data == null || listDetailXml2 == null || listDetailXml3 == null)
                    return rs = new ResultADO(false, "Khong Map duoc tu ADO sang XMLDetail", null);

                //create danh sach ChiTietThuoc
                XML2ListDetailData dsChiTietThuoc = new XML2ListDetailData();
                dsChiTietThuoc.CHI_TIET_THUOC = listDetailXml2;

                //Create danh sach ChiTietDVKT
                XML3ListDetailData dsChiTietDVKT = new XML3ListDetailData();
                dsChiTietDVKT.CHI_TIET_DVKT = listDetailXml3;

                rs = new ResultADO();
                rs.Success = true;
                rs.Data = new object[] { Xml1Data, dsChiTietThuoc, dsChiTietDVKT };
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return rs;
        }
    }
}
