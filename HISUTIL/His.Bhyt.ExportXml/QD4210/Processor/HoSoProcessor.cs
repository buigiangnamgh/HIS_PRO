using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.QD4210.ADO;
using His.Bhyt.ExportXml.QD4210.XML.HoSo;
using His.Bhyt.ExportXml.QD4210.XML.XML1;
using His.Bhyt.ExportXml.QD4210.XML.XML2;
using His.Bhyt.ExportXml.QD4210.XML.XML3;
using His.Bhyt.ExportXml.QD4210.XML.XML4;
using His.Bhyt.ExportXml.QD4210.XML.XML5;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.QD4210.Processor
{
    internal class HoSoProcessor : XmlProcessorBase
    {
        InputADO Data { get; set; }
        internal HoSoProcessor(InputADO data)
        {
            this.Data = data;
            if (data.ListHeinMediOrg != null && data.ListHeinMediOrg.Count > 0)
            {
                GlobalConfigStore.HisHeinMediOrg = data.ListHeinMediOrg;
            }

            if (data.ConfigData != null && data.ConfigData.Count > 0 && (GlobalConfigStore.ListIcdCode_Nds == null || GlobalConfigStore.ListIcdCode_Nds.Count <= 0))
            {
                string icdNds = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.NdsIcdCodeOtherCFG);
                if (!String.IsNullOrWhiteSpace(icdNds))
                {
                    GlobalConfigStore.ListIcdCode_Nds = icdNds.Split(',').ToList();
                }
            }

            if (data.ConfigData != null && data.ConfigData.Count > 0 && (GlobalConfigStore.ListIcdCode_Nds_Te == null || GlobalConfigStore.ListIcdCode_Nds_Te.Count <= 0))
            {
                string icdNds = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.NdsIcdCodeTeCFG);
                if (!String.IsNullOrWhiteSpace(icdNds))
                {
                    GlobalConfigStore.ListIcdCode_Nds_Te = icdNds.Split(',').ToList();
                }
            }
        }

        internal bool Processor()
        {
            bool result = false;
            try
            {
                var giamdinh = this.CreateGiamDinhHoSo();
                if (giamdinh == null || !giamdinh.Success || giamdinh.Data == null || giamdinh.Data.Length == 0)
                    return false;
                var fileName = string.Format("{0}_{1}_{2}", new string[] { DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss"), Data.Treatment.TREATMENT_CODE, Data.Treatment.TDL_PATIENT_CODE });
                if (!String.IsNullOrEmpty(GlobalConfigStore.PathSaveXml) && !Directory.Exists(GlobalConfigStore.PathSaveXml))
                {
                    Directory.CreateDirectory(GlobalConfigStore.PathSaveXml);
                }

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
                var fileName = string.Format("{0}_{1}_{2}", new string[] { DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss"), Data.Treatment.TREATMENT_CODE, Data.Treatment.TDL_PATIENT_CODE });
                //if (!String.IsNullOrEmpty(GlobalConfigStore.PathSaveXml) && !Directory.Exists(GlobalConfigStore.PathSaveXml))
                //{
                //    Directory.CreateDirectory(GlobalConfigStore.PathSaveXml);
                //}

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

        internal string ProcessorPath()
        {
            string result = "";
            try
            {
                var giamdinh = this.CreateGiamDinhHoSo();
                if (giamdinh == null || !giamdinh.Success || giamdinh.Data == null || giamdinh.Data.Length == 0)
                    return "";
                var fileName = string.Format("{0}_{1}_{2}", new string[] { DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss"), Data.Treatment.TREATMENT_CODE, Data.Treatment.TDL_PATIENT_CODE });
                if (!String.IsNullOrEmpty(GlobalConfigStore.PathSaveXml) && !Directory.Exists(GlobalConfigStore.PathSaveXml))
                {
                    Directory.CreateDirectory(GlobalConfigStore.PathSaveXml);
                }

                var path = string.Format("{0}/{1}.xml", GlobalConfigStore.PathSaveXml, fileName);
                var rs = this.CreatedXmlFile(giamdinh.Data[0] as GiamDinhHoSo, false, true, true, path);
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
                dsHoSo.HOSO = new List<HoSo> { hoSo.Data[0] as HoSo };
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
                if (file == null || !file.Success || file.Data == null || file.Data.Length < 5)
                    return rs = new ResultADO(false, "File ho so khong du 5 file", null);

                XML1Data dataXml1 = null;
                XML1DataPlus dataXml1Plus = null;
                if (file.Data[0] is XML1Data)
                {
                    dataXml1 = file.Data[0] as XML1Data;
                }
                else if (file.Data[0] is XML1DataPlus)
                {
                    dataXml1Plus = file.Data[0] as XML1DataPlus;
                }

                XML2Data dsChiTietThuoc = file.Data[1] as XML2Data;
                XML3Data dsChiTietDVKT = file.Data[2] as XML3Data;
                XML4Data dsChiTietCLS = file.Data[3] as XML4Data;
                XML5Data dsDienBienLS = file.Data[4] as XML5Data;

                bool xml1 = true;
                bool xml2 = true;
                bool xml3 = true;
                bool xml4 = true;
                bool xml5 = true;

                string XMLNumbers = "";
                if (Data.ConfigData != null && Data.ConfigData.Count > 0)
                {
                    XMLNumbers = HisConfigKey.GetConfigData(Data.ConfigData, HisConfigKey.XmlNumbers);
                }
                else
                {
                    XMLNumbers = Data.HeinServiceTypeCodeNoTutorial;
                }

                if (!String.IsNullOrWhiteSpace(XMLNumbers))
                {
                    List<string> nums = XMLNumbers.Split(',').ToList();
                    List<int> xmlNum = new List<int>();
                    foreach (var item in nums)
                    {
                        int n = 0;
                        if (int.TryParse(item, out n)) xmlNum.Add(n);
                    }

                    xml1 = xmlNum.Exists(o => o == 1);
                    xml2 = xmlNum.Exists(o => o == 2);
                    xml3 = xmlNum.Exists(o => o == 3);
                    xml4 = xmlNum.Exists(o => o == 4);
                    xml5 = xmlNum.Exists(o => o == 5);
                }

                var listFileXml = new List<FileHoSo>();
                //Create noi dung fileHoSo
                ResultADO createXml1File = null;
                if (dataXml1 != null)
                {
                    createXml1File = this.CreatedXmlFile(dataXml1, true, false, false, string.Empty);
                }
                else if (dataXml1Plus != null)
                {
                    createXml1File = this.CreatedXmlFile(dataXml1Plus, true, false, false, string.Empty);
                }

                if (createXml1File == null || !createXml1File.Success || createXml1File.Data == null || createXml1File.Data.Length == 0)
                    return new ResultADO(false, "Khong tao duoc file Xml 1", null);

                if (xml1)
                {
                    FileHoSo fileXml1 = new FileHoSo();
                    fileXml1.LOAIHOSO = XmlType.XML1;
                    fileXml1.NOIDUNGFILE = createXml1File.Data[0] as string;
                    listFileXml.Add(fileXml1);
                }

                if (xml2 && dsChiTietThuoc.CHI_TIET_THUOC != null && dsChiTietThuoc.CHI_TIET_THUOC.Count > 0)
                {
                    var createXml2File = this.CreatedXmlFile(dsChiTietThuoc, true, false, false, string.Empty);
                    if (createXml2File == null || !createXml2File.Success || createXml2File.Data == null || createXml2File.Data.Length == 0)
                        return new ResultADO(false, "Khong tao duoc file Xml 2", null);
                    FileHoSo fileXml2 = new FileHoSo();
                    fileXml2.LOAIHOSO = XmlType.XML2;
                    fileXml2.NOIDUNGFILE = createXml2File.Data[0] as string;
                    listFileXml.Add(fileXml2);
                }

                if (xml3 && dsChiTietDVKT.CHI_TIET_DVKT != null && dsChiTietDVKT.CHI_TIET_DVKT.Count > 0)
                {
                    var createXml3File = this.CreatedXmlFile(dsChiTietDVKT, true, false, false, string.Empty);
                    if (createXml3File == null || !createXml3File.Success || createXml3File.Data == null || createXml3File.Data.Length == 0)
                        return new ResultADO(false, "Khong tao duoc file Xml 3", null);
                    FileHoSo fileXml3 = new FileHoSo();
                    fileXml3.LOAIHOSO = XmlType.XML3;
                    fileXml3.NOIDUNGFILE = createXml3File.Data[0] as string;
                    listFileXml.Add(fileXml3);
                }

                if (xml4 && dsChiTietCLS.CHI_TIET_CLS != null && dsChiTietCLS.CHI_TIET_CLS.Count > 0)
                {
                    var createXml4File = this.CreatedXmlFile(dsChiTietCLS, true, false, false, string.Empty);
                    if (createXml4File == null || !createXml4File.Success || createXml4File.Data == null || createXml4File.Data.Length == 0)
                        return new ResultADO(false, "Khong tao duoc file Xml 4", null);
                    FileHoSo fileXml4 = new FileHoSo();
                    fileXml4.LOAIHOSO = XmlType.XML4;
                    fileXml4.NOIDUNGFILE = createXml4File.Data[0] as string;
                    listFileXml.Add(fileXml4);
                }

                if (xml5 && dsDienBienLS.CHI_TIET_DIEN_BIEN_BENH != null && dsDienBienLS.CHI_TIET_DIEN_BIEN_BENH.Count > 0)
                {
                    var createXml5File = this.CreatedXmlFile(dsDienBienLS, true, false, false, string.Empty);
                    if (createXml5File == null || !createXml5File.Success || createXml5File.Data == null || createXml5File.Data.Length == 0)
                        return new ResultADO(false, "Khong tao duoc file Xml 5", null);
                    FileHoSo fileXml5 = new FileHoSo();
                    fileXml5.LOAIHOSO = XmlType.XML5;
                    fileXml5.NOIDUNGFILE = createXml5File.Data[0] as string;
                    listFileXml.Add(fileXml5);
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
                object Xml1Data = null;
                List<XML2DetailData> listDetailXml2 = new List<XML2DetailData>();
                List<XML3DetailData> listDetailXml3 = new List<XML3DetailData>();
                List<XML4DetailData> listDetailXml4 = new List<XML4DetailData>();
                List<XML5DetailData> listDetailXml5 = new List<XML5DetailData>();
                //Xử lý xml2
                Xml2Processor xml2Processor = new Xml2Processor();
                var xml2Result = xml2Processor.GenerateXml2ADO(this.Data);
                if (xml2Result == null || !xml2Result.Success || xml2Result.Data == null)
                    return xml2Result;
                var listXmlThuocAdo = xml2Result.Data[0] as List<Xml2ADO>;

                //Xử lý xml 3
                Xml3Processor xml3Processor = new Xml3Processor();
                var xml3Result = xml3Processor.GenerateXml3ADO(this.Data);
                if (xml3Result == null || !xml3Result.Success || xml3Result.Data == null)
                    return xml3Result;
                var listXmlDvktVTAdo = xml3Result.Data[0] as List<Xml3ADO>;

                //xu ly xml 4
                Xml4Processor xml4Processor = new Xml4Processor();
                var xml4Result = xml4Processor.GenerateXml4ADO(this.Data);
                if (xml4Result == null || !xml4Result.Success || xml4Result.Data == null)
                    return xml4Result;
                var listXmlClsAdo = xml4Result.Data[0] as List<Xml4ADO>;

                //xu ly xml 5
                Xml5Processor xml5Processor = new Xml5Processor();
                var xml5Result = xml5Processor.GenerateXml5ADO(this.Data);
                if (xml5Result == null || !xml5Result.Success || xml5Result.Data == null)
                    return xml5Result;
                var listXmlDblsAdo = xml5Result.Data[0] as List<Xml5ADO>;

                //Xu ly Xml 1
                Xml1Processor xml1Procssor = new Xml1Processor();
                var xml1Result = xml1Procssor.GenerateXml1Data(this.Data, listXmlThuocAdo, listXmlDvktVTAdo);
                if (xml1Result == null || !xml1Result.Success || xml1Result.Data == null || xml1Result.Data.Length == 0)
                    return xml1Result;
                Xml1Data = xml1Result.Data[0];

                //process listXml2Detail, listXml3Detail
                xml2Processor.MapADOToXml(listXmlThuocAdo, ref listDetailXml2);
                xml3Processor.MapADOToXml(listXmlDvktVTAdo, ref listDetailXml3);
                xml4Processor.MapADOToXml(listXmlClsAdo, ref listDetailXml4);
                xml5Processor.MapADOToXml(listXmlDblsAdo, ref listDetailXml5);

                if (Xml1Data == null || listDetailXml2 == null || listDetailXml3 == null || listDetailXml4 == null || listDetailXml5 == null)
                    return rs = new ResultADO(false, "Khong Map duoc tu ADO sang XMLDetail", null);

                //create danh sach ChiTietThuoc
                XML2Data dsChiTietThuoc = new XML2Data();
                dsChiTietThuoc.CHI_TIET_THUOC = listDetailXml2;

                //Create danh sach ChiTietDVKT
                XML3Data dsChiTietDVKT = new XML3Data();
                dsChiTietDVKT.CHI_TIET_DVKT = listDetailXml3;

                //Create danh sach ChiTietCLS
                XML4Data dsChiTietCLS = new XML4Data();
                dsChiTietCLS.CHI_TIET_CLS = listDetailXml4;

                //Create danh sach DienBienLS
                XML5Data dsDienBienLS = new XML5Data();
                dsDienBienLS.CHI_TIET_DIEN_BIEN_BENH = listDetailXml5;

                rs = new ResultADO();
                rs.Success = true;
                rs.Data = new object[] { Xml1Data, dsChiTietThuoc, dsChiTietDVKT, dsChiTietCLS, dsDienBienLS };
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return rs;
        }
    }
}
