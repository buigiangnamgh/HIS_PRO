using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.QD4210.ADO;
using His.Bhyt.ExportXml.QD4210.XML.XML3;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.QD4210.Processor
{
    class Xml3Processor : XmlProcessorBase
    {
        private string giuongGhepOption;

        internal ResultADO GenerateXml3ADO(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                string MaterialPackageOption = "";
                string MaterialPriceOriginalOption = "";
                string MaterialStentRatio = "";
                string NgayYLenhOption = "";
                string NgayKetQuaOption = "";
                string MaBacSiOption = "";
                string MaterialStent2Limit = "";
                string gtOption = "";
                giuongGhepOption = "";
                string TtThauQd5937Option = "";
                string MaGiuongOption = "";
                string MaBacSiHeinServiceType = "";
                if (data.ConfigData != null && data.ConfigData.Count > 0)
                {
                    MaterialPackageOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.MOS__BHYT__CALC_MATERIAL_PACKAGE_PRICE_OPTION);
                    MaterialPriceOriginalOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.XML__4210__MATERIAL_PRICE_OPTION);
                    MaterialStentRatio = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.XML__4210__MATERIAL_STENT_RATIO_OPTION);
                    NgayYLenhOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.NgayYlenhOption);
                    NgayKetQuaOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.XML__4210__XML3__NGAY_KQ_OPTION);
                    MaBacSiOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.MaBacSiOption);
                    MaterialStent2Limit = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.Stent2LimitOption);
                    gtOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.GayTeOptionCFG);
                    giuongGhepOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.giuongGhepOptionCFG);
                    TtThauQd5937Option = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.TtThauQd5937OptionCFG);
                    MaGiuongOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.XML__MA_GIUONG_OPTION_CFG);
                    MaBacSiHeinServiceType = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.MA_BAC_SI_HEIN_SERVICE_TYPE);
                }
                else
                {
                    MaterialPackageOption = data.MaterialPackageOption;
                    MaterialPriceOriginalOption = data.MaterialPriceOriginalOption;
                    MaterialStentRatio = data.MaterialStentRatio;
                    MaBacSiOption = data.MaBacSiOption;
                    MaterialStent2Limit = data.MaterialStent2Limit;
                }

                List<Xml3ADO> listXml3Ado = new List<Xml3ADO>();
                var listHeinServiceType = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT
                };

                var listHeinServiceTypeMaterial = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                };

                var listServeservs = data.ListSereServ.Where(s => listHeinServiceType.Contains(s.TDL_HEIN_SERVICE_TYPE_ID.Value) || listHeinServiceTypeMaterial.Contains(s.TDL_HEIN_SERVICE_TYPE_ID.Value)).OrderBy(b => b.INTRUCTION_TIME).ToList();//lấy các dịch vụ không phải là thuốc, máu và có tỷ lệ chi trả tiền, các dịch vụ được hưởng BHYT

                List<V_HIS_SERE_SERV_2> listSereTemp = new List<V_HIS_SERE_SERV_2>();
                foreach (var item in listServeservs)
                {
                    if (item.PARENT_ID.HasValue && item.STENT_ORDER.HasValue)
                    {
                        var ortherStent = listServeservs.FirstOrDefault(o => o.PARENT_ID == item.PARENT_ID && item.STENT_ORDER.HasValue && o.STENT_ORDER.HasValue && o != item);
                        if (ortherStent != null)
                        {
                            if (item.STENT_ORDER < ortherStent.STENT_ORDER)
                            {
                                continue;
                            }
                            else if (item.STENT_ORDER > ortherStent.STENT_ORDER)
                            {
                                listSereTemp.Add(ortherStent);
                            }
                        }
                    }
                    listSereTemp.Add(item);
                }
                listServeservs = listSereTemp;

                Dictionary<long, List<V_HIS_SERE_SERV_2>> dicChildSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_2>>();
                foreach (var hisSereServ in listServeservs)
                {
                    if (hisSereServ.PARENT_ID.HasValue)
                    {
                        if (!dicChildSereServ.ContainsKey(hisSereServ.PARENT_ID.Value))
                            dicChildSereServ[hisSereServ.PARENT_ID.Value] = new List<V_HIS_SERE_SERV_2>();
                        dicChildSereServ[hisSereServ.PARENT_ID.Value].Add(hisSereServ);
                    }
                }

                var listHighTech = listServeservs.Where(o => dicChildSereServ.ContainsKey(o.ID)).OrderBy(b => b.INTRUCTION_TIME).ToList();
                Dictionary<long, string> dicHighTech = new Dictionary<long, string>();

                int count = 1;

                foreach (var hisSereServ in listServeservs)
                {
                    string maVatTu = "";
                    string maNhom = "";
                    string maDichVu = "";
                    string goiVTYT = "";
                    string tenVatTu = "";
                    string tenDichVu = "";
                    string ttThau = "";
                    string maGiuong = "";
                    string maBacSi = "";
                    string maKhoa = "";
                    //maNhom = hisSereServ.TDL_HST_BHYT_CODE ?? "";                   
                    maNhom = hisSereServ.HST_BHYT_CODE ?? "";
                    if (hisSereServ.HST_BHYT_CODE_IN_TIME.HasValue && data.Treatment.IN_TIME < hisSereServ.HST_BHYT_CODE_IN_TIME.Value)
                    {
                        maNhom = hisSereServ.OLD_HST_BHYT_CODE ?? (hisSereServ.HST_BHYT_CODE ?? "");
                    }

                    var xml3 = new Xml3ADO();
                    if (listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                    {
                        xml3.IsMaterial = true;
                        maVatTu = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        tenVatTu = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";

                        if (TtThauQd5937Option == "1")
                        {
                            List<string> ttThauVt = new List<string>();
                            if (!String.IsNullOrWhiteSpace(hisSereServ.MATERIAL_BID_EXTRA_CODE))
                            {
                                ttThauVt.Add(hisSereServ.MATERIAL_BID_EXTRA_CODE);
                            }
                            if (!String.IsNullOrWhiteSpace(hisSereServ.MATERIAL_BID_PACKAGE_CODE))
                            {
                                ttThauVt.Add(hisSereServ.MATERIAL_BID_PACKAGE_CODE);
                            }
                            if (!String.IsNullOrWhiteSpace(hisSereServ.MATERIAL_BID_GROUP_CODE))
                            {
                                ttThauVt.Add(hisSereServ.MATERIAL_BID_GROUP_CODE);
                            }
                            if (!String.IsNullOrWhiteSpace(hisSereServ.MATERIAL_BID_YEAR))
                            {
                                ttThauVt.Add(hisSereServ.MATERIAL_BID_YEAR);
                            }

                            if (ttThauVt.Count > 0)
                            {
                                ttThau = String.Join(";", ttThauVt);
                            }
                        }

                        //đảm báo có thông tin thầu
                        if (String.IsNullOrWhiteSpace(ttThau))
                        {
                            List<string> ttThauVt = new List<string>();
                            ttThauVt.Add(hisSereServ.MATERIAL_BID_YEAR);
                            ttThauVt.Add(!string.IsNullOrEmpty(hisSereServ.MATERIAL_BID_PACKAGE_CODE) ? hisSereServ.MATERIAL_BID_PACKAGE_CODE.Substring(hisSereServ.MATERIAL_BID_PACKAGE_CODE.Length - 2) : "01");
                            ttThauVt.Add(hisSereServ.MATERIAL_BID_NUMBER);

                            if (ttThauVt.Count > 0)
                            {
                                ttThau = String.Join(".", ttThauVt);
                            }
                        }

                        maKhoa = hisSereServ.REQUEST_BHYT_CODE ?? "";
                        tenDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";

                        if (hisSereServ.PARENT_ID.HasValue)
                        {
                            //Thêm điều kiện "phòng chỉ định" phải cùng "phòng xử lý" của dịch vụ cha (KTC hoặc PTTT)
                            var parent = listHighTech.FirstOrDefault(o => o.ID == hisSereServ.PARENT_ID.Value);
                            if (parent != null)
                            {
                                if (parent.TDL_EXECUTE_DEPARTMENT_ID == hisSereServ.TDL_REQUEST_DEPARTMENT_ID ||
                                    (MaterialPackageOption == "1"))
                                {
                                    maKhoa = parent.REQUEST_BHYT_CODE ?? "";
                                    maDichVu = parent.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                                    tenDichVu = parent.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                                    if (!dicHighTech.ContainsKey(hisSereServ.PARENT_ID.Value))
                                    {
                                        dicHighTech[hisSereServ.PARENT_ID.Value] = "G" + (dicHighTech.Count + 1);
                                    }

                                    goiVTYT = dicHighTech[hisSereServ.PARENT_ID.Value];
                                }
                            }
                        }
                        if (MaGiuongOption == "1" && data.BedLogs != null && data.BedLogs.Count > 0 && hisSereServ.TDL_INTRUCTION_TIME >= data.Treatment.CLINICAL_IN_TIME)
                        {
                            var bedlog = data.BedLogs.Where(o => o.START_TIME <= hisSereServ.TDL_INTRUCTION_TIME).OrderByDescending(o => o.START_TIME).FirstOrDefault();

                            if (bedlog != null)
                            {
                                maGiuong = bedlog.BED_CODE ?? "";
                            }
                            else
                            {
                                bedlog = data.BedLogs.Where(o => o.START_TIME >= hisSereServ.TDL_INTRUCTION_TIME).OrderBy(o => o.START_TIME).FirstOrDefault();

                                if (bedlog != null)
                                {
                                    maGiuong = bedlog.BED_CODE ?? "";
                                }

                            }
                        }
                    }
                    else
                    {
                        maKhoa = hisSereServ.REQUEST_BHYT_CODE ?? "";
                        maDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        tenDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                        if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT ||
                            hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT ||
                            hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN ||
                            hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                        {
                            maGiuong = ".";
                            if (data.BedLogs != null && data.BedLogs.Count > 0)
                            {
                                var bedlog = data.BedLogs.Where(o => o.SERVICE_REQ_ID.HasValue && o.SERVICE_REQ_ID.Value == hisSereServ.SERVICE_REQ_ID && o.BED_SERVICE_TYPE_ID == hisSereServ.SERVICE_ID && o.SHARE_COUNT == hisSereServ.SHARE_COUNT).ToList();

                                //nếu không có service_req_id thì tìm theo thời gian y lệnh
                                if (bedlog == null || bedlog.Count <= 0)
                                {
                                    bedlog = data.BedLogs.Where(o => o.ID == hisSereServ.BED_LOG_ID).ToList();
                                }

                                //nếu không có service_req_id thì tìm theo thời gian y lệnh
                                if (bedlog == null || bedlog.Count <= 0)
                                {
                                    bedlog = data.BedLogs.Where(o => o.BED_SERVICE_TYPE_ID == hisSereServ.SERVICE_ID && o.SHARE_COUNT == hisSereServ.SHARE_COUNT && o.START_TIME <= hisSereServ.TDL_INTRUCTION_TIME && (o.FINISH_TIME.HasValue && o.FINISH_TIME.Value >= hisSereServ.TDL_INTRUCTION_TIME)).ToList();
                                }

                                if (bedlog != null && bedlog.Count > 0)
                                {
                                    List<string> bedCodes = bedlog.Select(s => s.BED_CODE).Distinct().ToList();
                                    maGiuong = String.Join(";", bedCodes);
                                }
                            }
                        }
                        else if (MaGiuongOption == "1" && data.BedLogs != null && data.BedLogs.Count > 0 && hisSereServ.TDL_INTRUCTION_TIME >= data.Treatment.CLINICAL_IN_TIME)
                        {
                            var bedlog = data.BedLogs.Where(o => o.START_TIME <= hisSereServ.TDL_INTRUCTION_TIME).OrderByDescending(o => o.START_TIME).FirstOrDefault();

                            if (bedlog != null)
                            {
                                maGiuong = bedlog.BED_CODE ?? "";
                            }
                            else
                            {
                                bedlog = data.BedLogs.Where(o => o.START_TIME >= hisSereServ.TDL_INTRUCTION_TIME).OrderBy(o => o.START_TIME).FirstOrDefault();

                                if (bedlog != null)
                                {
                                    maGiuong = bedlog.BED_CODE ?? "";
                                }

                            }
                        }
                    }

                    xml3.MaLienKet = data.Treatment.TREATMENT_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                    xml3.Stt = count;
                    xml3.MaDichVu = maDichVu;
                    xml3.MaVatTu = maVatTu;
                    xml3.MaNhom = maNhom;
                    xml3.GoiVTYT = goiVTYT;
                    xml3.TenVatTu = tenVatTu;
                    xml3.TenDichVu = tenDichVu;
                    //xml3.TenDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                    xml3.DonViTinh = hisSereServ.SERVICE_UNIT_NAME ?? "";

                    xml3.SoLuong = Math.Round(hisSereServ.AMOUNT, 3, MidpointRounding.AwayFromZero);
                    xml3.TongNguonKhac = Math.Round(xml3.SoLuong * (hisSereServ.OTHER_SOURCE_PRICE ?? 0), 2, MidpointRounding.AwayFromZero);

                    xml3.TTThau = ttThau;
                    decimal tyle = 0;
                    if (hisSereServ.ORIGINAL_PRICE > 0)
                    {
                        tyle = hisSereServ.HEIN_LIMIT_PRICE.HasValue ? (hisSereServ.HEIN_LIMIT_PRICE.Value / (hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO))) * 100 : (hisSereServ.PRICE / hisSereServ.ORIGINAL_PRICE) * 100;
                    }

                    xml3.TyLeTT = Math.Round(tyle, 0);
                    xml3.MucHuong = hisSereServ.HEIN_RATIO.HasValue ? (int)(hisSereServ.HEIN_RATIO.Value * 100) : 0;

                    decimal chenhLechVatTu = 0;
                    //_____ #17251:
                    //Sửa lại, y/c, với vật tư thì trường DON_GIA theo giá bán (PRICE * (1 + VAT)) 
                    //TYLE_TT thì vẫn tính như hiện tại
                    //___#20575: Cấu hình lấy giá trần hay giá bán.
                    //cấu hình = 1 thì lấy giá bán
                    if (listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value) && hisSereServ.PRICE > hisSereServ.ORIGINAL_PRICE && MaterialPriceOriginalOption == "1")
                    {
                        chenhLechVatTu = (hisSereServ.PRICE - hisSereServ.ORIGINAL_PRICE) * (1 + hisSereServ.VAT_RATIO) * hisSereServ.AMOUNT;
                    }

                    if (chenhLechVatTu > 0)
                    {
                        xml3.DonGia = Math.Round(hisSereServ.PRICE * (1 + hisSereServ.VAT_RATIO), 3, MidpointRounding.AwayFromZero);
                    }
                    else
                        xml3.DonGia = Math.Round(hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO), 3, MidpointRounding.AwayFromZero);

                    //đơn vị quy đổi #16449
                    //đổi trước khi tính tiền để không bị lệch thành tiền do số lượng nhỏ bị làm tròn
                    if (hisSereServ.CONVERT_RATIO.HasValue && hisSereServ.USE_ORIGINAL_UNIT_FOR_PRES != 1)
                    {
                        xml3.DonViTinh = hisSereServ.CONVERT_UNIT_NAME ?? "";
                        xml3.SoLuong = Math.Round(hisSereServ.AMOUNT * hisSereServ.CONVERT_RATIO.Value, 3, MidpointRounding.AwayFromZero);
                        if (chenhLechVatTu > 0)
                        {
                            xml3.DonGia = Math.Round((hisSereServ.PRICE * (1 + hisSereServ.VAT_RATIO)) / hisSereServ.CONVERT_RATIO.Value, 3, MidpointRounding.AwayFromZero);
                        }
                        else
                            xml3.DonGia = Math.Round((hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO)) / hisSereServ.CONVERT_RATIO.Value, 3, MidpointRounding.AwayFromZero);
                    }

                    //tinh tien bhyt
                    ProcessPrice(data, hisSereServ, ref xml3, chenhLechVatTu);

                    //làm tròn TongBHTT và TongBNCCT dẫn đến âm TongBNTT
                    // dồn lại tiền TongBNCCT
                    if (xml3.TongBNTT < 0)
                    {
                        xml3.TongBNTT = 0;
                        xml3.TongBNCCT = xml3.ThanhTien - xml3.TongBHTT - xml3.TongNguonKhac;
                    }

                    xml3.PhamVi = 1;
                    if (listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                    {
                        //xml3.TongTranTT = 0;
                        if (hisSereServ.HEIN_LIMIT_PRICE.HasValue)
                        {
                            xml3.TongTranTT = Math.Round(hisSereServ.HEIN_LIMIT_PRICE ?? 0, 2, MidpointRounding.AwayFromZero);

                            //#32976
                            if (hisSereServ.STENT_ORDER.HasValue && MaterialStent2Limit == "1")
                            {
                                var stends = listServeservs.Where(o => o.PARENT_ID == hisSereServ.PARENT_ID && o.STENT_ORDER.HasValue).OrderBy(o => o.STENT_ORDER).ToList();
                                if (stends != null && stends.Count > 1)
                                {
                                    //stend 2
                                    if (stends[1].STENT_ORDER == hisSereServ.STENT_ORDER && (!hisSereServ.CONFIG_HEIN_LIMIT_PRICE.HasValue || hisSereServ.HEIN_LIMIT_PRICE < ((hisSereServ.CONFIG_HEIN_LIMIT_PRICE ?? 0) / 2)))
                                    {
                                        xml3.TongTranTT = null;
                                    }
                                }
                            }
                        }
                        else if (hisSereServ.STENT_ORDER.HasValue && MaterialStent2Limit == "2")
                        {
                            var stends = listServeservs.Where(o => o.PARENT_ID == hisSereServ.PARENT_ID && o.STENT_ORDER.HasValue).OrderBy(o => o.STENT_ORDER).ToList();
                            if (stends != null && stends.Count > 0)
                            {
                                if (stends[0].STENT_ORDER == hisSereServ.STENT_ORDER)
                                {
                                    xml3.TongTranTT = Math.Round(hisSereServ.CONFIG_HEIN_LIMIT_PRICE ?? 0, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (stends[1].STENT_ORDER == hisSereServ.STENT_ORDER)
                                {
                                    xml3.TongTranTT = Math.Round(xml3.DonGia / 2, 2, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    xml3.TongTranTT = null;
                                }
                            }
                        }

                        if (xml3.TongTranTT.HasValue)
                        {
                            xml3.TongTranTT = Math.Round(xml3.TongTranTT.Value * xml3.SoLuong, 2, MidpointRounding.AwayFromZero);
                        }

                        //nếu là vật tư và có cấu hình
                        //tỷ lệ thanh toán đối với stent luôn là 100
                        if (MaterialStentRatio == "1" && data.MaterialTypes != null && data.MaterialTypes.Count > 0)
                        {
                            var material = data.MaterialTypes.FirstOrDefault(o => o.SERVICE_ID == hisSereServ.SERVICE_ID);
                            if (material.IS_STENT == 1)
                            {
                                xml3.TyLeTT = 100;
                            }
                        }

                        if (xml3.TongBHTT == 0)
                        {
                            xml3.PhamVi = 2;
                            xml3.TyLeTT = 0;
                        }
                    }

                    //Ngoai dinh suat
                    MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL heinApproval = null;
                    if (data.HeinApprovals != null && data.HeinApprovals.Count > 0)
                    {
                        heinApproval = data.HeinApprovals.FirstOrDefault(o => o.ID == hisSereServ.HEIN_APPROVAL_ID);
                    }

                    if (heinApproval == null)
                    {
                        heinApproval = data.HeinApproval;
                    }

                    if (base.CheckBhytNsd(GlobalConfigStore.ListIcdCode_Nds, GlobalConfigStore.ListIcdCode_Nds_Te,
                     hisSereServ.ICD_CODE, heinApproval, hisSereServ.SERVICE_ID, data.TotalSericeData, data.TotalIcdData))
                    {
                        xml3.TongNgoaiDS = xml3.TongBHTT;
                    }
                    else if (base.CheckBhytNsd(GlobalConfigStore.ListIcdCode_Nds, GlobalConfigStore.ListIcdCode_Nds_Te, data.Treatment, heinApproval))
                    {
                        xml3.TongNgoaiDS = xml3.TongBHTT;
                    }
                    else
                    {
                        xml3.TongNgoaiDS = 0;
                    }

                    xml3.MaKhoa = maKhoa;// hisSereServ.REQUEST_BHYT_CODE ?? "";
                    xml3.MaGiuong = maGiuong.Trim();
                    if (String.IsNullOrEmpty(MaBacSiHeinServiceType) || (!String.IsNullOrEmpty(MaBacSiHeinServiceType) && data.ListHeinServiceType != null && data.ListHeinServiceType.Count > 0 && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.HasValue && data.ListHeinServiceType.FirstOrDefault(o=>o.ID == hisSereServ.TDL_HEIN_SERVICE_TYPE_ID) != null && data.ListHeinServiceType.FirstOrDefault(o => o.ID == hisSereServ.TDL_HEIN_SERVICE_TYPE_ID).HEIN_SERVICE_TYPE_CODE != MaBacSiHeinServiceType))
                    {
                        List<String> lstMaBacSi = ProcessorGetMaBacSi(hisSereServ, MaBacSiOption, data.EkipUsers, listHeinServiceTypeMaterial, data.Employees);

                        if (MaBacSiOption == "4")
                            maBacSi = string.Join(";", lstMaBacSi.ToList());
                        else
                            maBacSi = string.Join(";", lstMaBacSi.Distinct().ToList());
                    }
                    xml3.MaBacSi = maBacSi ?? "";

                    List<string> mabenh = new List<string>();
                    if (!String.IsNullOrWhiteSpace(hisSereServ.ICD_CODE))
                    {
                        mabenh.Add(hisSereServ.ICD_CODE);
                    }

                    if (!String.IsNullOrWhiteSpace(hisSereServ.ICD_SUB_CODE))
                    {
                        var benh = hisSereServ.ICD_SUB_CODE.Trim(';').Split(';').ToList();
                        mabenh.AddRange(benh);
                    }

                    if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_CODE))
                    {
                        mabenh.Add(data.Treatment.ICD_CODE);
                    }

                    if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_SUB_CODE))
                    {
                        var benh = data.Treatment.ICD_SUB_CODE.Trim(';').Split(';').ToList();
                        mabenh.AddRange(benh);
                    }

                    mabenh = mabenh.Distinct().ToList();
                    xml3.MaBenh = string.Join(";", mabenh);

                    //thời gian bắt đầu và thời gian kết thúc trong xử lý PTTT, SA, XQ là thời gian từng dịch vụ HIS_SERE_SERV_EXT
                    string time_kq = hisSereServ.END_TIME.HasValue ? hisSereServ.END_TIME.ToString().Substring(0, 12) : hisSereServ.FINISH_TIME.HasValue ? hisSereServ.FINISH_TIME.ToString().Substring(0, 12) : hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);
                    string time_yl = hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);

                    string start_time = hisSereServ.START_TIME.HasValue ? hisSereServ.START_TIME.ToString().Substring(0, 12) : hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);

                    if (NgayYLenhOption == "3")
                    {
                        time_yl = start_time;
                    }
                    if (NgayKetQuaOption == "1" && hisSereServ.FINISH_TIME.HasValue)
                    {
                        time_kq = hisSereServ.FINISH_TIME < hisSereServ.INTRUCTION_TIME ? hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12) : hisSereServ.FINISH_TIME.ToString().Substring(0, 12);
                    }
                    else if (NgayKetQuaOption == "2")
                    {
                        time_kq = hisSereServ.FINISH_TIME.HasValue ? hisSereServ.FINISH_TIME.ToString().Substring(0, 12) : "";
                    }

                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT ||
                        hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT ||
                        hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN ||
                        hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                    {
                        var bed = data.BedLogs != null && data.BedLogs.Count > 0 ? data.BedLogs.FirstOrDefault(o => o.ID == hisSereServ.BED_LOG_ID) : null;
                        time_kq = bed != null && bed.FINISH_TIME.HasValue ? bed.FINISH_TIME.ToString().Substring(0, 12) : time_kq;
                        if (hisSereServ.BED_LOG_ID.HasValue)
                        {
                            var sereServs = data.ListSereServ.Where(o => hisSereServ.BED_LOG_ID.Value == o.BED_LOG_ID).ToList();
                            if (sereServs != null && sereServs.Count > 1)
                            {
                                var sere = sereServs.Where(o => o.TDL_INTRUCTION_TIME > hisSereServ.TDL_INTRUCTION_TIME).ToList();
                                var betterIntructionTime = sere != null && sere.Count > 0 ? sere.Min(o => o.TDL_INTRUCTION_TIME) : 0;

                                if (betterIntructionTime > 0)
                                {
                                    time_kq = betterIntructionTime.ToString().Substring(0, 12);
                                }
                            }
                        }
                    }
                    else if (hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && NgayYLenhOption == "2")
                    {
                        time_yl = start_time;
                    }
                    else if (hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT ||
                        hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                    {
                        time_yl = hisSereServ.BEGIN_TIME.HasValue ? hisSereServ.BEGIN_TIME.ToString().Substring(0, 12) : time_yl;
                        time_kq = hisSereServ.END_TIME.HasValue ? hisSereServ.END_TIME.ToString().Substring(0, 12) : time_kq;
                    }

                    xml3.NgayYLenh = time_yl;
                    xml3.NgayKetQua = time_kq;

                    //sua loi iss 11615
                    if (hisSereServ.PARENT_ID.HasValue)
                    {
                        var parent = listServeservs.FirstOrDefault(o => o.ID == hisSereServ.PARENT_ID.Value);
                        if (parent != null)
                        {
                            xml3.NgayYLenh = parent.INTRUCTION_TIME.ToString().Substring(0, 12);
                            if ((parent.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT
                                || parent.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                                && parent.BEGIN_TIME.HasValue)
                            {
                                if (hisSereServ.END_TIME > 0 && parent.BEGIN_TIME > hisSereServ.END_TIME)
                                {
                                    xml3.NgayYLenh = hisSereServ.BEGIN_TIME.ToString().Substring(0, 12);
                                }
                                else
                                {
                                    xml3.NgayYLenh = parent.BEGIN_TIME.ToString().Substring(0, 12);
                                }
                            }
                        }
                    }

                    if (xml3.TongNgoaiDS > 0)
                    {
                        xml3.MaPTTT = 2;
                    }
                    else
                    {
                        xml3.MaPTTT = 1;
                    }

                    if (!String.IsNullOrWhiteSpace(gtOption) && data.SereServPttts != null && data.SereServPttts.Count > 0)
                    {
                        var ssPttt = data.SereServPttts.FirstOrDefault(o => o.SERE_SERV_ID == hisSereServ.ID);
                        if (ssPttt != null && ssPttt.IS_ANAESTHESIA == 1)
                        {
                            //mã dịch vụ đã có _GT
                            if (gtOption != "2")
                            {
                                xml3.TenDichVu += "[gây tê]";
                                xml3.MaDichVu += "_GT";
                            }
                        }
                    }

                    listXml3Ado.Add(xml3);
                    count++;
                }
                rs = new ResultADO(true, "", new object[] { listXml3Ado });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private List<string> ProcessorGetMaBacSi(V_HIS_SERE_SERV_2 hisSereServ, string MaBacSiOption, List<HIS_EKIP_USER> ekipUsers, List<long> listHeinServiceTypeMaterial, List<HIS_EMPLOYEE> employees)
        {
            List<string> lstMaBacSi = new List<string>();
            try
            {
                string executeName = null;
                string reqName = null;
                string samplerLoginName = null;
                List<string> resultName = new List<string>();
                if (MaBacSiOption == "4")
                {
                    if (hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G || hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT)
                    {
                        string cchn = GetMaBacSi(hisSereServ.REQUEST_LOGINNAME);
                        if (!String.IsNullOrWhiteSpace(cchn))
                        {
                            lstMaBacSi.Add(cchn);
                        }
                    }
                    else
                    {
                        string cchnA = GetMaBacSi(hisSereServ.REQUEST_LOGINNAME);
                        if (!String.IsNullOrWhiteSpace(cchnA))
                        {
                            lstMaBacSi.Add(cchnA);
                        }
                        if (hisSereServ.EKIP_ID.HasValue && ekipUsers != null && ekipUsers.Count > 0)
                        {
                            //có kíp add theo kíp
                            var dataEkip = ekipUsers.Where(o => o.EKIP_ID == hisSereServ.EKIP_ID.Value).ToList();
                            if (dataEkip != null && dataEkip.Count > 0)
                            {
                                foreach (var item in dataEkip)
                                {
                                    string cchn = GetMaBacSi(item.LOGINNAME);
                                    if (!String.IsNullOrWhiteSpace(cchn))
                                    {
                                        lstMaBacSi.Add(cchn);
                                    }
                                }
                            }
                        }
                        else if (!String.IsNullOrWhiteSpace(hisSereServ.SUBCLINICAL_RESULT_LOGINNAME))
                        {
                            string[] loginname = hisSereServ.SUBCLINICAL_RESULT_LOGINNAME.Split(';');
                            var lstTmp = new List<string>();
                            foreach (var item in loginname)
                            {
                                string cchn = GetMaBacSi(item);
                                if (!String.IsNullOrWhiteSpace(cchn))
                                {
                                    lstTmp.Add(cchn);
                                }
                            }
                            lstMaBacSi.AddRange(lstTmp.Distinct().ToList());
                        }
                        else
                        {
                            string cchnB = GetMaBacSi(hisSereServ.EXECUTE_LOGINNAME);
                            if (!String.IsNullOrWhiteSpace(cchnB))
                            {
                                lstMaBacSi.Add(cchnB);
                            }
                        }
                    }
                }
                else
                {
                    executeName = GetMaBacSi(hisSereServ.EXECUTE_LOGINNAME);
                    reqName = GetMaBacSi(hisSereServ.REQUEST_LOGINNAME);
                    samplerLoginName = GetMaBacSi(hisSereServ.SAMPLER_LOGINNAME);  //Người lấy mẫu
                    resultName = new List<string>();
                    if (MaBacSiOption == "5"
                        && hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN
                        && !String.IsNullOrWhiteSpace(hisSereServ.RECEIVE_SAMPLE_LOGINNAME))
                    {
                        samplerLoginName = GetMaBacSi(hisSereServ.RECEIVE_SAMPLE_LOGINNAME);
                    }
                    else
                        if (!String.IsNullOrWhiteSpace(hisSereServ.SUBCLINICAL_RESULT_LOGINNAME))
                    {
                        string[] loginname = hisSereServ.SUBCLINICAL_RESULT_LOGINNAME.Split(';');
                        foreach (var item in loginname)
                        {
                            string cchn = GetMaBacSi(item);
                            if (!String.IsNullOrWhiteSpace(cchn))
                            {
                                resultName.Add(cchn);
                            }
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(executeName) || resultName.Count > 0)
                    {
                        if (resultName.Count > 0)
                        {
                            lstMaBacSi.AddRange(resultName);
                        }
                        else
                        {
                            lstMaBacSi.Add(executeName);
                        }
                    }
                    else if (!String.IsNullOrWhiteSpace(reqName))
                    {
                        lstMaBacSi.Add(reqName);
                    }

                    if (hisSereServ.EKIP_ID.HasValue && ekipUsers != null && ekipUsers.Count > 0)
                    {
                        //có kíp add theo kíp
                        var dataEkip = ekipUsers.Where(o => o.EKIP_ID == hisSereServ.EKIP_ID.Value).Distinct().ToList();
                        if (dataEkip != null && dataEkip.Count > 0)
                        {
                            lstMaBacSi = new List<string>();
                            //thêm nã bác sĩ yêu cầu.
                            if (!String.IsNullOrWhiteSpace(reqName))
                            {
                                lstMaBacSi.Add(reqName);
                            }

                            foreach (var item in dataEkip)
                            {
                                string cchn = GetMaBacSi(item.LOGINNAME);
                                if (!String.IsNullOrWhiteSpace(cchn))
                                {
                                    lstMaBacSi.Add(cchn);
                                }
                            }
                        }
                    }
                    else if (listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0))
                    {
                        lstMaBacSi = new List<string>();
                        if (!String.IsNullOrWhiteSpace(reqName))
                        {
                            lstMaBacSi.Add(reqName);
                        }
                        else if (!String.IsNullOrWhiteSpace(executeName))
                        {
                            lstMaBacSi.Add(executeName);
                        }
                    }
                    else if (MaBacSiOption == "2")
                    {
                        //key giá trị 2 add cả 2
                        lstMaBacSi = new List<string>();
                    }
                    else if ((MaBacSiOption == "3" || MaBacSiOption == "5") &&
                        (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH
                        && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN
                        && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L
                        && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT
                        && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT))
                    {
                        // key giá trị 3 thì ngoài thuốc, vật tư, giường. tất cả sẽ add 2 cái nếu có
                        lstMaBacSi = new List<string>();
                    }
                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH && hisSereServ.TDL_IS_MAIN_EXAM != 1 && (MaBacSiOption == "1" || MaBacSiOption == "3" || MaBacSiOption == "5"))
                    {
                        //ko phải khám chính thì add cả 2 nếu có cấu hình
                        lstMaBacSi = new List<string>();
                    }
                }
                if (lstMaBacSi.Count == 0)
                {
                    if (!String.IsNullOrWhiteSpace(reqName))
                    {
                        lstMaBacSi.Add(reqName);
                    }
                    if (!String.IsNullOrWhiteSpace(samplerLoginName) && (MaBacSiOption == "1" || MaBacSiOption == "2" || MaBacSiOption == "3" || MaBacSiOption == "5"))
                    {
                        lstMaBacSi.Add(samplerLoginName);    //Người lấy mẫu
                    }

                    if (!String.IsNullOrWhiteSpace(executeName) || resultName.Count > 0)
                    {
                        if (resultName.Count > 0)
                        {
                            lstMaBacSi.AddRange(resultName);
                        }
                        else
                        {
                            lstMaBacSi.Add(executeName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return lstMaBacSi;
        }

        private void ProcessPrice(InputADO data, V_HIS_SERE_SERV_2 hisSereServ, ref Xml3ADO xml3, decimal chenhLechVatTu)
        {
            try
            {
                var listLiveArea = new List<string>
                {
                    MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K1,
                    MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K2,
                    MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K3
                };

                decimal TRAITUYEN = 0;
                if (!String.IsNullOrWhiteSpace(data.HeinApproval.LIVE_AREA_CODE) && listLiveArea.Contains(data.HeinApproval.LIVE_AREA_CODE))
                {
                    TRAITUYEN = 100;
                }
                else if (data.HeinApproval.LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE)
                {
                    if ((hisSereServ.HEIN_RATIO ?? 0) > 0.6m)
                        TRAITUYEN = 100;
                    else
                        TRAITUYEN = 60;
                }
                else if (data.HeinApproval.LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL)
                {
                    TRAITUYEN = 40;
                }

                if ((xml3.MaNhom == "15" /*&& (xml3.TyLeTT == 50 || xml3.TyLeTT == 30)*/)
                    || (xml3.MaNhom == "13" /*&& (xml3.TyLeTT == 30 || xml3.TyLeTT == 10)*/)
                    || ((xml3.MaNhom == "8" || xml3.MaNhom == "18") && (xml3.TyLeTT == 50 || xml3.TyLeTT == 80))
                    || data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE
                    || (giuongGhepOption == "2" && xml3.MaNhom == "14" && hisSereServ.SHARE_COUNT.HasValue))
                {
                    xml3.ThanhTien = Math.Round(xml3.SoLuong * xml3.DonGia * (xml3.TyLeTT / 100), 2, MidpointRounding.AwayFromZero);
                    xml3.TongBHTT = Math.Round((xml3.ThanhTien - chenhLechVatTu) * (hisSereServ.HEIN_RATIO ?? 0), 2, MidpointRounding.AwayFromZero);
                    xml3.TongBNCCT = Math.Round((xml3.ThanhTien - chenhLechVatTu) - xml3.TongBHTT, 2, MidpointRounding.AwayFromZero);

                    //cong thuc tinh tien cv 1/6/2018
                    //cong thuc khong de cap den noi tru ngoai tru ma chi co trai tuyen
                    if (data.HeinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE && TRAITUYEN != 0)
                    {
                        xml3.TongBNCCT = Math.Round((xml3.ThanhTien - chenhLechVatTu) * TRAITUYEN / 100, 2, MidpointRounding.AwayFromZero) - xml3.TongBHTT;
                    }
                }
                else
                {
                    xml3.ThanhTien = Math.Round(xml3.SoLuong * xml3.DonGia, 2, MidpointRounding.AwayFromZero);
                    if (giuongGhepOption == "1" && xml3.MaNhom == "14" && hisSereServ.SHARE_COUNT.HasValue)
                    {
                        xml3.ThanhTien = Math.Round(xml3.SoLuong * xml3.DonGia * (xml3.TyLeTT / 100), 2, MidpointRounding.AwayFromZero);
                    }

                    xml3.TongBHTT = Math.Round((xml3.ThanhTien - chenhLechVatTu) * (hisSereServ.HEIN_RATIO ?? 0) * (xml3.TyLeTT / 100), 2, MidpointRounding.AwayFromZero);
                    xml3.TongBNCCT = Math.Round((xml3.ThanhTien - chenhLechVatTu) * (xml3.TyLeTT / 100), 2, MidpointRounding.AwayFromZero) - xml3.TongBHTT;

                    if (hisSereServ.MATERIAL_ID.HasValue && !CheckSamePrice(Math.Round(hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0, 0), Math.Round(xml3.TongBHTT, 0)))
                    {
                        xml3.TongBHTT = Math.Round(hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0, 2, MidpointRounding.AwayFromZero);
                        xml3.TongBNCCT = Math.Round(hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0, 2, MidpointRounding.AwayFromZero);
                    }

                    if (data.HeinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                    {
                        if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC && CheckQuyenLoiCard(hisSereServ.HEIN_CARD_NUMBER))
                        {
                            xml3.TongBNCCT = (xml3.ThanhTien - chenhLechVatTu) - xml3.TongBHTT;
                        }
                        else if (TRAITUYEN != 0)
                        {
                            xml3.TongBNCCT = Math.Round((xml3.ThanhTien - chenhLechVatTu) * (xml3.TyLeTT / 100) * TRAITUYEN / 100, 2, MidpointRounding.AwayFromZero) - xml3.TongBHTT;
                        }
                    }
                }

                xml3.TongBNTT = xml3.ThanhTien - xml3.TongBNCCT - xml3.TongBHTT - xml3.TongNguonKhac;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool CheckQuyenLoiCard(string cardNumber)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(cardNumber))
                {
                    var quyenloi = cardNumber.Substring(2, 1);
                    if (quyenloi == "1" || quyenloi == "2" || quyenloi == "5")
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool CheckSamePrice(decimal p1, decimal p2)
        {
            bool result = false;
            if (p1 == p2 || (p1 - 1) == p2 || (p1 + 1) == p2) result = true;
            return result;
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
                else
                {
                    Inventec.Common.Logging.LogSystem.Info("ListEmployees null");
                }
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal void MapADOToXml(List<Xml3ADO> listAdo, ref List<XML3DetailData> datas)
        {
            try
            {
                if (datas == null)
                    datas = new List<XML3DetailData>();
                if (listAdo != null || listAdo.Count > 0)
                {
                    foreach (var ado in listAdo)
                    {
                        XML3DetailData detail = new XML3DetailData();
                        detail.DON_GIA = ado.DonGia.ToString("G27", CultureInfo.InvariantCulture);
                        detail.DON_VI_TINH = ado.DonViTinh;
                        detail.GOI_VTYT = ado.GoiVTYT;
                        detail.MA_BAC_SI = ado.MaBacSi;
                        detail.MA_BENH = ado.MaBenh;
                        detail.MA_DICH_VU = ado.MaDichVu;
                        detail.MA_GIUONG = ado.MaGiuong;
                        detail.MA_KHOA = ado.MaKhoa;
                        detail.MA_LK = ado.MaLienKet;
                        detail.MA_NHOM = ado.MaNhom;
                        detail.MA_PTTT = ado.MaPTTT;
                        detail.MA_VAT_TU = ado.MaVatTu;
                        detail.MUC_HUONG = ado.MucHuong;
                        detail.NGAY_YL = ado.NgayYLenh;
                        detail.NGAY_KQ = ado.NgayKetQua;
                        detail.PHAM_VI = ado.PhamVi;
                        detail.SO_LUONG = ado.SoLuong.ToString("G27", CultureInfo.InvariantCulture);
                        detail.STT = ado.Stt;
                        detail.T_BHTT = ado.TongBHTT.ToString("G27", CultureInfo.InvariantCulture);
                        detail.T_BNCCT = ado.TongBNCCT.ToString("G27", CultureInfo.InvariantCulture);
                        detail.T_BNTT = ado.TongBNTT.ToString("G27", CultureInfo.InvariantCulture);
                        detail.T_NGOAIDS = ado.TongNgoaiDS.ToString("G27", CultureInfo.InvariantCulture);
                        detail.T_NGUONKHAC = ado.TongNguonKhac.ToString("G27", CultureInfo.InvariantCulture);
                        if (ado.TongTranTT.HasValue)
                            detail.T_TRANTT = ado.TongTranTT.Value.ToString("G27", CultureInfo.InvariantCulture);
                        else
                            detail.T_TRANTT = "";

                        detail.TEN_DICH_VU = this.ConvertStringToXmlDocument(ado.TenDichVu);
                        detail.TEN_VAT_TU = this.ConvertStringToXmlDocument(ado.TenVatTu);
                        detail.THANH_TIEN = ado.ThanhTien.ToString("G27", CultureInfo.InvariantCulture);
                        detail.TT_THAU = ado.TTThau;
                        detail.TYLE_TT = ado.TyLeTT.ToString("G27", CultureInfo.InvariantCulture);
                        datas.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                datas = null;
            }
        }
    }
}
