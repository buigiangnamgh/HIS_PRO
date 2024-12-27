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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.QD4210.Processor
{
    class HoSoGroupProcessor : XmlProcessorBase
    {
        InputGroupADO Data { get; set; }
        internal HoSoGroupProcessor(InputGroupADO data)
        {
            this.Data = data;
        }

        internal string ProcessorPath(ref string messageError)
        {
            string result = "";
            try
            {
                var giamdinh = this.CreateGiamDinhHoSo();
                if (giamdinh == null || !giamdinh.Success || giamdinh.Data == null || giamdinh.Data.Length == 0)
                    return "";
                var fileName = string.Format("{0}_{1}", new string[] { "Data", DateTime.Now.ToString("yyyyMMdd_HHmmss") });
                var path = string.Format("{0}/{1}.xml", GlobalConfigStore.PathSaveXml, fileName);
                messageError = giamdinh.Message;
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
                rs = new ResultADO(true, ttHoSo.Message, new object[] { giamdinh });
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
                thongtinHoSo.DANHSACHHOSO = dsHoso.Data[0] as DanhSachHoSo;
                thongtinHoSo.SOLUONGHOSO = thongtinHoSo.DANHSACHHOSO.HOSO != null ? thongtinHoSo.DANHSACHHOSO.HOSO.Count : 0;
                rs = new ResultADO(true, dsHoso.Message, new object[] { thongtinHoSo });
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
                var hoSo = this.CreateListHoSo();
                if (hoSo == null || !hoSo.Success || hoSo.Data == null || hoSo.Data.Length == 0)
                    throw new Exception("Tao FileHoSo that bai: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => hoSo), hoSo));
                DanhSachHoSo dsHoSo = new DanhSachHoSo();
                dsHoSo.HOSO = hoSo.Data[0] as List<HoSo>;
                rs = new ResultADO(true, hoSo.Message, new object[] { dsHoSo });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateListHoSo()
        {
            ResultADO rs = null;
            try
            {
                Dictionary<string, List<string>> DicErrorMess = new Dictionary<string, List<string>>();
                List<HoSo> listHoso = new List<HoSo>();
                foreach (var treatment in this.Data.Treatments)
                {
                    InputADO data = new InputADO();
                    data.Treatment = treatment;
                    if (this.Data.HeinApprovals != null && this.Data.HeinApprovals.Count > 0)
                    {
                        var heinApprovals = this.Data.HeinApprovals.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                        if (heinApprovals != null && heinApprovals.Count > 0)
                        {
                            data.HeinApproval = heinApprovals.OrderByDescending(o => o.EXECUTE_TIME ?? 0).ThenByDescending(o => o.ID).FirstOrDefault();
                            data.HeinApprovals = heinApprovals;
                        }
                    }

                    if (this.Data.ListSereServ != null && this.Data.ListSereServ.Count > 0)
                    {
                        data.ListSereServ = this.Data.ListSereServ.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();
                    }

                    if (this.Data.EkipUsers != null && this.Data.EkipUsers.Count > 0 && data.ListSereServ != null && data.ListSereServ.Count > 0)
                    {
                        List<long> ekip = data.ListSereServ.Select(s => s.EKIP_ID ?? 0).Distinct().ToList();
                        if (ekip != null && ekip.Count > 0)
                        {
                            data.EkipUsers = this.Data.EkipUsers.Where(o => ekip.Contains(o.ID)).ToList();
                        }
                    }

                    if (this.Data.BedLogs != null && this.Data.BedLogs.Count > 0)
                    {
                        data.BedLogs = this.Data.BedLogs.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                    }

                    if (this.Data.Dhsts != null && this.Data.Dhsts.Count > 0)
                    {
                        var dhst = this.Data.Dhsts.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                        if (dhst != null && dhst.Count > 0)
                        {
                            data.Dhst = dhst.OrderByDescending(o => o.ID).FirstOrDefault(o => o.WEIGHT.HasValue) ?? dhst.OrderByDescending(o => o.ID).FirstOrDefault();
                        }
                    }

                    if (this.Data.SereServPttts != null && this.Data.SereServPttts.Count > 0)
                    {
                        data.SereServPttts = this.Data.SereServPttts.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();
                    }

                    if (this.Data.SereServTeins != null && this.Data.SereServTeins.Count > 0)
                    {
                        data.SereServTeins = this.Data.SereServTeins.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();
                    }

                    if (this.Data.Trackings != null && this.Data.Trackings.Count > 0)
                    {
                        data.Trackings = this.Data.Trackings.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                    }

                    if (this.Data.ListDebate != null && this.Data.ListDebate.Count > 0)
                    {
                        data.ListDebate = this.Data.ListDebate.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                    }

                    if (this.Data.ListDhsts != null && this.Data.ListDhsts.Count > 0)
                    {
                        data.ListDhsts = this.Data.ListDhsts.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                    }

                    data.Branch = this.Data.Branch;
                    data.MaterialTypes = this.Data.MaterialTypes;
                    data.HeinServiceTypeCodeNoTutorial = this.Data.HeinServiceTypeCodeNoTutorial;
                    data.IsTreatmentDayCount6556 = this.Data.IsTreatmentDayCount6556;
                    data.MaterialPackageOption = this.Data.MaterialPackageOption;
                    data.MaterialPriceOriginalOption = this.Data.MaterialPriceOriginalOption;
                    data.MaterialStent2Limit = this.Data.MaterialStent2Limit;
                    data.MaterialStentRatio = this.Data.MaterialStentRatio;
                    data.TenBenhOption = this.Data.TenBenhOption;
                    data.XMLNumbers = this.Data.XMLNumbers;
                    data.ListHeinMediOrg = this.Data.ListHeinMediOrg;
                    data.ConfigData = this.Data.ConfigData;
                    data.TotalSericeData = this.Data.TotalSericeData;
                    data.TotalIcdData = this.Data.TotalIcdData;

                    string messageError = "";
                    string codeError = "";
                    if (Check(data, true, ref messageError, ref codeError))
                    {
                        var fileHoSo = this.CreateHoSo(data);
                        if (fileHoSo == null || !fileHoSo.Success || fileHoSo.Data == null || fileHoSo.Data.Length == 0)
                        {
                            Inventec.Common.Logging.LogSystem.Error("Tao FileHoSo that bai: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => fileHoSo), fileHoSo));
                            continue;
                        }

                        listHoso.Add(fileHoSo.Data[0] as HoSo);
                    }

                    if (!String.IsNullOrWhiteSpace(messageError))
                    {
                        if (!DicErrorMess.ContainsKey(messageError))
                        {
                            DicErrorMess[messageError] = new List<string>();
                        }

                        if (!String.IsNullOrWhiteSpace(codeError))
                        {
                            DicErrorMess[messageError].Add(string.Format("{0}({1})", treatment.TREATMENT_CODE, codeError));
                        }
                        else
                        {
                            DicErrorMess[messageError].Add(treatment.TREATMENT_CODE);
                        }
                    }
                }

                string error = "";
                if (DicErrorMess.Count > 0)
                {
                    foreach (var item in DicErrorMess)
                    {
                        error += String.Format("{0}:{1}. ", item.Key, String.Join(",", item.Value));
                    }
                }

                rs = new ResultADO(true, error, new object[] { listHoso });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateHoSo(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                var fileHoSo = this.CreateFileHoSo(data);
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

        private ResultADO CreateFileHoSo(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                var file = this.CreateNoiDungFile(data);
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
                if (!String.IsNullOrWhiteSpace(Data.XMLNumbers))
                {
                    List<string> nums = Data.XMLNumbers.Split(',').ToList();
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

        private ResultADO CreateNoiDungFile(InputADO data)
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
                var xml2Result = xml2Processor.GenerateXml2ADO(data);
                if (xml2Result == null || !xml2Result.Success || xml2Result.Data == null)
                    return xml2Result;
                var listXmlThuocAdo = xml2Result.Data[0] as List<Xml2ADO>;

                //Xử lý xml 3
                Xml3Processor xml3Processor = new Xml3Processor();
                var xml3Result = xml3Processor.GenerateXml3ADO(data);
                if (xml3Result == null || !xml3Result.Success || xml3Result.Data == null)
                    return xml3Result;
                var listXmlDvktVTAdo = xml3Result.Data[0] as List<Xml3ADO>;

                //xu ly xml 4
                Xml4Processor xml4Processor = new Xml4Processor();
                var xml4Result = xml4Processor.GenerateXml4ADO(data);
                if (xml4Result == null || !xml4Result.Success || xml4Result.Data == null)
                    return xml4Result;
                var listXmlClsAdo = xml4Result.Data[0] as List<Xml4ADO>;

                //xu ly xml 5
                Xml5Processor xml5Processor = new Xml5Processor();
                var xml5Result = xml5Processor.GenerateXml5ADO(data);
                if (xml5Result == null || !xml5Result.Success || xml5Result.Data == null)
                    return xml5Result;
                var listXmlDblsAdo = xml5Result.Data[0] as List<Xml5ADO>;

                //Xu ly Xml 1
                Xml1Processor xml1Procssor = new Xml1Processor();
                var xml1Result = xml1Procssor.GenerateXml1Data(data, listXmlThuocAdo, listXmlDvktVTAdo);
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

        private bool Check(InputADO entity, bool checkPath, ref string MessageError, ref string CodeError)
        {
            bool valid = true;
            try
            {
                if (entity == null)
                {
                    MessageError = "Lỗi dữ liệu khởi tạo";
                    throw new NullReferenceException("InputADO truyen vao null: ");
                }

                if (entity.HeinApproval == null && (entity.HeinApprovals == null || entity.HeinApprovals.Count == 0))
                {
                    MessageError = "Hồ sơ chưa giám định bhyt";
                    throw new NullReferenceException("HeinApprovalBhyt and entity.HeinApprovals is null: ");
                }

                if (entity.HeinApproval == null)
                    entity.HeinApproval = entity.HeinApprovals.FirstOrDefault();

                if (entity.ListSereServ == null || entity.ListSereServ.Count == 0)
                {
                    MessageError = "Hồ sơ không có dịch vụ";
                    throw new NullReferenceException("ListSereServ is null or empty: ");
                }

                if (entity.Treatment == null)
                {
                    MessageError = "Không xác định được hồ sơ điều trị";
                    throw new NullReferenceException("Treatment is null: ");
                }

                if (!entity.HeinApproval.EXECUTE_TIME.HasValue)
                {
                    MessageError = "Hồ sơ chưa có thời gian giám định bhyt";
                    throw new NullReferenceException("HeinApprovalBhyt.ExecuteTime null");
                }

                if (!entity.Treatment.FEE_LOCK_TIME.HasValue)
                {
                    MessageError = "Hồ sơ chưa có thời gian khóa viện phí";
                    throw new NullReferenceException("Treatment.FeeLockTime null");
                }

                if (entity.Treatment.FEE_LOCK_TIME.Value > entity.HeinApproval.EXECUTE_TIME.Value)
                {
                    MessageError = "Thời gian duyệt khóa viện phí lớn hơn thời gian duyệt giám định BHYT";
                    throw new NullReferenceException("Thoi gian duyet khoa vien phi lon hon thoi gia duyet giam dinh Bhyt");
                }

                if (!entity.Treatment.OUT_TIME.HasValue)
                {
                    MessageError = "Hồ sơ chưa có thời gian kết thúc điều trị";
                    throw new NullReferenceException("Treatment.OUT_TIME null");
                }

                if (entity.Treatment.FEE_LOCK_TIME.Value < entity.Treatment.OUT_TIME.Value)
                {
                    MessageError = "Thời gian ra viện lớn hơn thời gian thanh toán";
                    throw new NullReferenceException("Treatment.OUT_TIME > Treatment.FEE_LOCK_TIME");
                }

                if (!entity.Treatment.END_DEPARTMENT_ID.HasValue)
                {
                    MessageError = "Hồ sơ điểu trị không có khoa kết thúc điều trị";
                    throw new NullReferenceException("Khoa ket thuc dieu tri khong co");
                }

                if (!entity.Treatment.TREATMENT_END_TYPE_ID.HasValue)
                {
                    MessageError = "Hồ sơ điểu trị không có loại ra viện";
                    throw new NullReferenceException("Ho so dieu tri khong co TreatmentEndTypeId");
                }

                if (!entity.Treatment.TREATMENT_RESULT_ID.HasValue)
                {
                    MessageError = "Hồ sơ điểu trị không có kết quả";
                    throw new NullReferenceException("Ho so dieu tri khong co TreatmentResultId");
                }

                if (String.IsNullOrEmpty(entity.HeinApproval.RIGHT_ROUTE_CODE))
                {
                    MessageError = "Không xác định được diện đúng tuyến, trái tuyến";
                    throw new NullReferenceException("Khong Co HeinApprovalBhyt.RIGHT_ROUTE_CODE");
                }

                if (String.IsNullOrWhiteSpace(entity.Treatment.ICD_CODE))
                {
                    MessageError = "Hồ sơ không có thông tin bệnh";
                    throw new NullReferenceException("Khong Co Treatment.ICD_CODE");
                }

                if (entity.HeinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT && (!entity.Treatment.CLINICAL_IN_TIME.HasValue))
                {
                    MessageError = "Hồ sơ điều trị nội trú nhưng không có thời gian vào điều trị";
                    throw new NullReferenceException("Ho so dieu tri noi tru nhung khong co thoi gian vao dieu tri:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => entity.Treatment.CLINICAL_IN_TIME), entity.Treatment.CLINICAL_IN_TIME));
                }

                valid = valid && this.CheckConfigData(checkPath, ref MessageError);

                if (valid)
                {
                    List<long> listHeinServiceTypeId = new List<long>
                    {
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT
                    };

                    entity.ListSereServ = entity.ListSereServ.Where(o =>
                        o.TDL_HEIN_SERVICE_TYPE_ID.HasValue &&
                        o.PRICE > 0 &&
                        o.AMOUNT > 0 &&
                        o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&
                        o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&
                        o.VIR_TOTAL_HEIN_PRICE.HasValue &&
                        o.VIR_TOTAL_HEIN_PRICE > 0 &&
                        listHeinServiceTypeId.Contains(o.TDL_HEIN_SERVICE_TYPE_ID.Value)).ToList();

                    if (entity.ListSereServ == null || entity.ListSereServ.Count == 0)
                    {
                        MessageError = "Hồ sơ không có dịch vụ BHYT thanh toán";
                        throw new NullReferenceException("ListSereServ is null or empty sau khi da loc HeinServiceType,IsExpend: ");
                    }

                    CheckDetailData(entity.Treatment, entity.ListSereServ, ref MessageError, ref CodeError);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private void CheckDetailData(MOS.EFMODEL.DataModels.V_HIS_TREATMENT_3 Treatment, List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV_2> listSereServ, ref string messageError, ref string CodeError)
        {
            List<string> serviceReqCodeBs = new List<string>();
            List<string> serviceReqCodeLieuDung = new List<string>();
            List<string> codeNoTutorial = new List<string>();
            if (!String.IsNullOrWhiteSpace(this.Data.HeinServiceTypeCodeNoTutorial))
            {
                codeNoTutorial = this.Data.HeinServiceTypeCodeNoTutorial.Split('|').ToList();
            }

            var listHeinServiceTypeTh = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM,
                };

            if (listSereServ.Exists(o => String.IsNullOrWhiteSpace(o.ICD_CODE)))
            {
                messageError = "thiếu thông tin mã bệnh";
                CodeError = string.Join(",", listSereServ.Where(o => String.IsNullOrWhiteSpace(o.ICD_CODE)).Select(s => s.TDL_SERVICE_REQ_CODE).Distinct().ToList());
                throw new NullReferenceException("Tồn tại dịch vụ không có mã bệnh chính: " + CodeError);
            }

            if (listSereServ.Exists(o => o.TDL_INTRUCTION_TIME > Treatment.OUT_TIME))
            {
                messageError = "thời gian chỉ định sau thời gian ra viện";
                CodeError = string.Join(",", listSereServ.Where(o => o.TDL_INTRUCTION_TIME > Treatment.OUT_TIME).Select(s => s.TDL_SERVICE_REQ_CODE).Distinct().ToList());
                throw new NullReferenceException("Tồn tại dịch vụ có thời gian y lệnh lớn hơn thời gian ra viện: " + CodeError);
            }

            if (listSereServ.Exists(o => o.TDL_INTRUCTION_TIME < Treatment.IN_TIME))
            {
                messageError = "thời gian chỉ định trước thời gian vào viện";
                CodeError = string.Join(",", listSereServ.Where(o => o.TDL_INTRUCTION_TIME < Treatment.IN_TIME).Select(s => s.TDL_SERVICE_REQ_CODE).Distinct().ToList());
                throw new NullReferenceException("Tồn tại dịch vụ có thời gian chỉ định trước thời gian vào viện: " + CodeError);
            }

            if (listSereServ.Exists(o => o.TDL_INTRUCTION_TIME > Treatment.FEE_LOCK_TIME))
            {
                messageError = "thời gian chỉ định sau thời gian thanh toán";
                CodeError = string.Join(",", listSereServ.Where(o => o.TDL_INTRUCTION_TIME > Treatment.FEE_LOCK_TIME).Select(s => s.TDL_SERVICE_REQ_CODE).Distinct().ToList());
                throw new NullReferenceException("Tồn tại dịch vụ có thời gian y lệnh lớn hơn thời gian khóa viện phí: " + CodeError);
            }

            foreach (var item in listSereServ)
            {
                if (listHeinServiceTypeTh.Contains(item.TDL_HEIN_SERVICE_TYPE_ID ?? 0))
                {
                    if (!codeNoTutorial.Contains(item.ACTIVE_INGR_BHYT_CODE) && !codeNoTutorial.Contains(item.TDL_HEIN_SERVICE_BHYT_CODE))
                    {
                        if (String.IsNullOrWhiteSpace(item.TUTORIAL) && (item.TDL_HEIN_SERVICE_TYPE_ID ?? 0) != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU && (item.TDL_HEIN_SERVICE_TYPE_ID ?? 0) != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
                        {
                            serviceReqCodeLieuDung.Add(item.TDL_SERVICE_REQ_CODE);
                        }
                    }
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(item.EXECUTE_LOGINNAME) && String.IsNullOrWhiteSpace(item.REQUEST_LOGINNAME))
                    {
                        serviceReqCodeBs.Add(item.TDL_SERVICE_REQ_CODE);
                    }
                    else
                    {
                        var executeName = GetMaBacSi(item.EXECUTE_LOGINNAME);
                        var reqName = GetMaBacSi(item.REQUEST_LOGINNAME);
                        if (String.IsNullOrWhiteSpace(executeName) && String.IsNullOrWhiteSpace(reqName))
                        {
                            serviceReqCodeBs.Add(item.TDL_SERVICE_REQ_CODE);
                        }
                    }
                }
            }

            if (serviceReqCodeBs != null && serviceReqCodeBs.Count > 0)
            {
                messageError = "thiếu thông tin bác sĩ";
                serviceReqCodeBs = serviceReqCodeBs.Distinct().ToList();
                CodeError = string.Join(",", serviceReqCodeBs);
                throw new NullReferenceException("khong co ma bac si: " + CodeError);
            }

            if (serviceReqCodeLieuDung != null && serviceReqCodeLieuDung.Count > 0)
            {
                messageError = "thiếu thông tin liều dùng";
                serviceReqCodeLieuDung = serviceReqCodeLieuDung.Distinct().ToList();
                CodeError = string.Join(",", serviceReqCodeLieuDung);
                throw new NullReferenceException("khong co ma lieu dung: " + CodeError);
            }
        }

        private string GetMaBacSi(string loginName)
        {
            string result = "";
            try
            {
                if (GlobalConfigStore.ListEmployees != null)
                {
                    var dataEmployee = GlobalConfigStore.ListEmployees.FirstOrDefault(p => p.LOGINNAME == loginName);
                    if (dataEmployee != null)
                    {
                        result = dataEmployee.DIPLOMA;
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool CheckConfigData(bool checkPath, ref string messageError)
        {
            bool valid = true;
            try
            {
                if (this.Data.Branch == null && GlobalConfigStore.Branch == null)
                {
                    messageError = "Không xác định được chi nhánh của hồ sơ";
                    throw new NullReferenceException("Chua Xet Branch (Chi nhanh) cho LocalStore hoac InputADO");
                }

                if (checkPath && String.IsNullOrEmpty(GlobalConfigStore.PathSaveXml))
                {
                    messageError = "Không xác định được thư mục lưu file xml kết quả";
                    throw new NullReferenceException("Chua xet thu muc luu xml (PathSaveXml) cho LocalStore");
                }
            }
            catch (Exception ex)
            {
                valid = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return valid;
        }
    }
}
