using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FlexCel.Report;
using HIS.Desktop.Common.BankQrCode;
using Inventec.Common.BarcodeLib;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Common.Number;
using Inventec.Common.String;
using Inventec.Core;
using Inventec.Fss.Client;
using Inventec.UC.Login.Base;
using MOS.EFMODEL.DataModels;
using MPS.Processor.Mps000302.ADO;
using MPS.Processor.Mps000302.MPS.Processor.Mps000302.ADO;
using MPS.Processor.Mps000302.PDO;
using MPS.ProcessorBase.Core;

namespace MPS.Processor.Mps000302
{
    public class Mps000302Processor : AbstractProcessor
    {
        private class ReplaceValueFunction : TFlexCelUserFunction
        {
            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length == 0)
                {
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
                }
                try
                {
                    object obj = parameters[0];
                    string text = ((obj != null) ? obj.ToString() : null) ?? "";
                    if (!string.IsNullOrEmpty(text))
                    {
                        text = text.Replace(';', '/');
                    }
                    return text;
                }
                catch (Exception ex)
                {
                    LogSystem.Error(ex);
                    return parameters[0];
                }
            }
        }

        private Mps000302PDO rdo;

        private PatientADO patientADO { get; set; }

        private List<PatyAlterBhytADO> patyAlterBHYTADOs { get; set; }

        private List<SereServADO> sereServADOs { get; set; }

        private List<SereServADO> sereServADOSAs { get; set; }

        private List<HeinServiceTypeADO> heinServiceTypeADOs { get; set; }

        private List<MedicineLineADO> medicineLineADOs { get; set; }

        private List<HeinServiceTypeADO> HeinServiceTypeBeds { get; set; }

        private List<SereServADO> sereServADONoExamZero { get; set; }
        private List<GroupDepartmentADO> GroupDepartments { get; set; }

        public Mps000302Processor(CommonParam param, PrintData printData)
            : base(param, printData)
        {
            rdo = (Mps000302PDO)rdoBase;
        }

        private void SetBarcodeKey()
        {
            try
            {
                Barcode barcode = new Barcode(rdo.Treatment.TREATMENT_CODE);
                barcode.Alignment = AlignmentPositions.CENTER;
                barcode.IncludeLabel = false;
                barcode.Width = 120;
                barcode.Height = 40;
                barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
                barcode.LabelPosition = LabelPositions.BOTTOMCENTER;
                barcode.EncodedType = TYPE.CODE128;
                barcode.IncludeLabel = true;
                dicImage.Add("TREATMENT_CODE_BAR", barcode);
                Barcode barcode2 = new Barcode(rdo.Treatment.TDL_PATIENT_CODE);
                barcode2.Alignment = AlignmentPositions.CENTER;
                barcode2.IncludeLabel = false;
                barcode2.Width = 120;
                barcode2.Height = 40;
                barcode2.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
                barcode2.LabelPosition = LabelPositions.BOTTOMCENTER;
                barcode2.EncodedType = TYPE.CODE128;
                barcode2.IncludeLabel = true;
                dicImage.Add("PATIENT_CODE_BAR", barcode2);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SetImageKey()
        {
            try
            {
                bool flag = true;
                if (rdo.Patient != null && !string.IsNullOrEmpty(rdo.Patient.AVATAR_URL))
                {
                    SetSingleImage("IMG_AVATAR", rdo.Patient.AVATAR_URL);
                    flag = false;
                }
                if (rdo.CurrentPatyAlter != null && !string.IsNullOrEmpty(rdo.CurrentPatyAlter.BHYT_URL))
                {
                    SetSingleImage("IMG_BHYT", rdo.CurrentPatyAlter.BHYT_URL);
                    flag = false;
                }
                else if (rdo.Patient != null && !string.IsNullOrEmpty(rdo.Patient.BHYT_URL))
                {
                    SetSingleImage("IMG_BHYT", rdo.Patient.BHYT_URL);
                    flag = false;
                }
                if (flag)
                {
                    SetSingleKey("AVT_AND_BHYT_NULL", "1");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void SetSingleImage(string key, string imageUrl)
        {
            try
            {
                MemoryStream file = FileDownload.GetFile(imageUrl);
                if (file != null)
                {
                    SetSingleKey(new KeyValue(key, file.ToArray()));
                }
                else
                {
                    SetSingleKey(new KeyValue(key, ""));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        public override bool ProcessData()
        {
            bool flag = false;
            try
            {
                ProcessSingleTag processSingleTag = new ProcessSingleTag();
                ProcessBarCodeTag processBarCodeTag = new ProcessBarCodeTag();
                ProcessObjectTag processObjectTag = new ProcessObjectTag();
                store.ReadTemplate(Path.GetFullPath(fileName));
                DataInputProcess();
                GroupDisplayProcess();
                ProcessSingleKey();
                SetQrCode();
                SetBarcodeKey();
                SetImageKey();
                if (sereServADOs == null || sereServADOs.Count == 0)
                {
                    return false;
                }
                processSingleTag.ProcessData(store, singleValueDictionary);
                processBarCodeTag.ProcessData(store, dicImage);
                processObjectTag.AddObjectData(store, "HeinServiceType", heinServiceTypeADOs);
                processObjectTag.AddObjectData(store, "Service", sereServADOs);
                processObjectTag.AddObjectData(store, "PatyAlterBHYT", patyAlterBHYTADOs);
                processObjectTag.AddObjectData(store, "PatyAlterBHYTAll", patyAlterBHYTADOs);
                processObjectTag.AddObjectData(store, "MedicineLine", medicineLineADOs);
                processObjectTag.AddObjectData(store, "HeinServiceTypeBed", HeinServiceTypeBeds);
                processObjectTag.AddObjectData(store, "ServiceSA", sereServADOSAs);
                processObjectTag.AddObjectData(store, "ReqExeDepartment", GroupDepartments);
                processObjectTag.AddRelationship(store, "PatyAlterBHYT", "HeinServiceType", "KEY", "KEY_PATY_ALTER");
                processObjectTag.AddRelationship(store, "PatyAlterBHYT", "MedicineLine", "KEY", "KEY_PATY_ALTER");
                processObjectTag.AddRelationship(store, "PatyAlterBHYT", "HeinServiceTypeBed", "KEY", "KEY_PATY_ALTER");
                processObjectTag.AddRelationship(store, "PatyAlterBHYT", "Service", "KEY", "KEY_PATY_ALTER");
                processObjectTag.AddRelationship(store, "PatyAlterBHYT", "ServiceSA", "KEY", "KEY_PATY_ALTER");
                processObjectTag.AddRelationship(store, "PatyAlterBHYT", "ReqExeDepartment", "KEY", "KEY_PATY_ALTER");
                processObjectTag.AddRelationship(store, "HeinServiceType", "MedicineLine", "ID", "HEIN_SERVICE_TYPE_ID");
                processObjectTag.AddRelationship(store, "HeinServiceType", "HeinServiceTypeBed", "ID", "PARENT_ID");
                processObjectTag.AddRelationship(store, "HeinServiceType", "Service", "ID", "HEIN_SERVICE_TYPE_ID");
                processObjectTag.AddRelationship(store, "HeinServiceType", "ServiceSA", "ID", "HEIN_SERVICE_TYPE_ID");
                processObjectTag.AddRelationship(store, "MedicineLine", "HeinServiceTypeBed", "ID", "MEDICINE_LINE_ID");
                processObjectTag.AddRelationship(store, "MedicineLine", "Service", "ID", "MEDICINE_LINE_ID");
                processObjectTag.AddRelationship(store, "MedicineLine", "ServiceSA", "ID", "MEDICINE_LINE_ID");
                processObjectTag.AddRelationship(store, "HeinServiceTypeBed", "Service", "ID", "HEIN_SERVICE_TYPE_PARENT_1_ID");
                processObjectTag.AddRelationship(store, "HeinServiceTypeBed", "ServiceSA", "ID", "HEIN_SERVICE_TYPE_PARENT_1_ID");
                processObjectTag.AddRelationship(store, "ReqExeDepartment", "HeinServiceType", "GROUP_DEPARTMENT_ID", "GROUP_DEPARTMENT_ID");
                processObjectTag.AddRelationship(store, "ReqExeDepartment", "Service", "GROUP_DEPARTMENT_ID", "GROUP_DEPARTMENT_ID");
                processObjectTag.AddRelationship(store, "ReqExeDepartment", "HeinServiceTypeBed", "GROUP_DEPARTMENT_ID", "GROUP_DEPARTMENT_ID");
                processObjectTag.AddRelationship(store, "ReqExeDepartment", "MedicineLine", "GROUP_DEPARTMENT_ID", "GROUP_DEPARTMENT_ID");
                processObjectTag.SetUserFunction(store, "ReplaceValue", new ReplaceValueFunction());
                ProcssGroupHeinType(processObjectTag, store);
                ProcessNoExamZero(processObjectTag, store);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                LogSystem.Error(ex);
            }
            return flag;
        }

        private static void AggregateTotals(IEnumerable<HeinServiceTypeADO> sourceItems, HeinServiceTypeADO target)
        {
            try
            {
                if (sourceItems == null || target == null)
                {
                    return;
                }
                PropertyInfo[] array = (from p in typeof(HeinServiceTypeADO).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                        where p.CanRead && p.CanWrite && p.Name.StartsWith("TOTAL", StringComparison.OrdinalIgnoreCase)
                                        select p).ToArray();
                PropertyInfo[] array2 = array;
                foreach (PropertyInfo propertyInfo in array2)
                {
                    Type propertyType = propertyInfo.PropertyType;
                    Type type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                    if (type == typeof(decimal))
                    {
                        decimal num2 = default(decimal);
                        foreach (HeinServiceTypeADO sourceItem in sourceItems)
                        {
                            object value = propertyInfo.GetValue(sourceItem);
                            if (value != null)
                            {
                                num2 += (decimal)value;
                            }
                        }
                        propertyInfo.SetValue(target, (propertyType == type) ? ((object)num2) : ((object)num2));
                    }
                    else if (type == typeof(double))
                    {
                        double num3 = 0.0;
                        foreach (HeinServiceTypeADO sourceItem2 in sourceItems)
                        {
                            object value2 = propertyInfo.GetValue(sourceItem2);
                            if (value2 != null)
                            {
                                num3 += (double)value2;
                            }
                        }
                        propertyInfo.SetValue(target, (propertyType == type) ? ((object)num3) : ((object)num3));
                    }
                    else if (type == typeof(float))
                    {
                        float num4 = 0f;
                        foreach (HeinServiceTypeADO sourceItem3 in sourceItems)
                        {
                            object value3 = propertyInfo.GetValue(sourceItem3);
                            if (value3 != null)
                            {
                                num4 += (float)value3;
                            }
                        }
                        propertyInfo.SetValue(target, (propertyType == type) ? ((object)num4) : ((object)num4));
                    }
                    else if (type == typeof(long))
                    {
                        long num5 = 0L;
                        foreach (HeinServiceTypeADO sourceItem4 in sourceItems)
                        {
                            object value4 = propertyInfo.GetValue(sourceItem4);
                            if (value4 != null)
                            {
                                num5 += (long)value4;
                            }
                        }
                        propertyInfo.SetValue(target, (propertyType == type) ? ((object)num5) : ((object)num5));
                    }
                    else
                    {
                        if (!(type == typeof(int)))
                        {
                            continue;
                        }
                        int num6 = 0;
                        foreach (HeinServiceTypeADO sourceItem5 in sourceItems)
                        {
                            object value5 = propertyInfo.GetValue(sourceItem5);
                            if (value5 != null)
                            {
                                num6 += (int)value5;
                            }
                        }
                        propertyInfo.SetValue(target, (propertyType == type) ? ((object)num6) : ((object)num6));
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void ProcssGroupHeinType(ProcessObjectTag objectTag, Store store)
        {
            try
            {
                objectTag.AddObjectData(store, "HeinServiceTypeOnly", (from o in heinServiceTypeADOs
                                                                       group o by new { o.NUM_ORDER, o.HEIN_SERVICE_TYPE_NAME, o.HEIN_SERVICE_TYPE_CHILD_NUM_ORDER }).Select(g =>
                {
                    HeinServiceTypeADO heinServiceTypeADO = g.First();
                    AggregateTotals(g, heinServiceTypeADO);
                    return heinServiceTypeADO;
                }).ToList());
                objectTag.AddRelationship(store, "HeinServiceTypeOnly", "Service", "ID", "HEIN_SERVICE_TYPE_ID");
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessNoExamZero(ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (sereServADONoExamZero != null && sereServADONoExamZero.Count > 0)
                {
                    List<PatyAlterBhytADO> listData = null;
                    List<HeinServiceTypeADO> listData2 = null;
                    List<HeinServiceTypeADO> HeinServiceTypeBeds = null;
                    List<MedicineLineADO> listData3 = null;
                    GroupDisplayProcess(sereServADONoExamZero, ref listData, ref listData2, ref HeinServiceTypeBeds, ref listData3);
                    objectTag.AddObjectData(store, "HeinServiceTypeNoExamZero", listData2);
                    objectTag.AddObjectData(store, "ServiceNoExamZero", sereServADONoExamZero);
                    objectTag.AddObjectData(store, "PatyAlterBHYTNoExamZero", listData);
                    objectTag.AddObjectData(store, "HeinServiceTypeBedNoExamZero", HeinServiceTypeBeds);
                    objectTag.AddObjectData(store, "MedicineLineNoExamZero", listData3);
                    objectTag.AddRelationship(store, "PatyAlterBHYTNoExamZero", "ServiceNoExamZero", "KEY", "KEY_PATY_ALTER");
                    objectTag.AddRelationship(store, "PatyAlterBHYTNoExamZero", "HeinServiceTypeNoExamZero", "KEY", "KEY_PATY_ALTER");
                    objectTag.AddRelationship(store, "PatyAlterBHYTNoExamZero", "HeinServiceTypeBedNoExamZero", "KEY", "KEY_PATY_ALTER");
                    objectTag.AddRelationship(store, "PatyAlterBHYTNoExamZero", "MedicineLineNoExamZero", "KEY", "KEY_PATY_ALTER");
                    objectTag.AddRelationship(store, "HeinServiceTypeNoExamZero", "MedicineLineNoExamZero", "ID", "HEIN_SERVICE_TYPE_ID");
                    objectTag.AddRelationship(store, "HeinServiceTypeNoExamZero", "HeinServiceTypeBedNoExamZero", "ID", "PARENT_ID");
                    objectTag.AddRelationship(store, "HeinServiceTypeNoExamZero", "ServiceNoExamZero", "ID", "HEIN_SERVICE_TYPE_ID");
                    objectTag.AddRelationship(store, "MedicineLineNoExamZero", "HeinServiceTypeBedNoExamZero", "ID", "MEDICINE_LINE_ID");
                    objectTag.AddRelationship(store, "MedicineLineNoExamZero", "ServiceNoExamZero", "ID", "MEDICINE_LINE_ID");
                    objectTag.AddRelationship(store, "HeinServiceTypeBedNoExamZero", "ServiceNoExamZero", "ID", "HEIN_SERVICE_TYPE_PARENT_1_ID");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessSingleKey()
        {
            try
            {
                SetSingleKey(new KeyValue("RATIO_STR", rdo.SingleKeyValue.ratio));
                SetSingleKey(new KeyValue("TREATMENT_CODE", rdo.Treatment.TREATMENT_CODE));
                SetSingleKey(new KeyValue("USERNAME_RETURN_RESULT", rdo.SingleKeyValue.userNameReturnResult));
                SetSingleKey(new KeyValue("STATUS_TREATMENT_OUT", rdo.SingleKeyValue.statusTreatmentOut));
                int? num;
                string value;
                if (rdo.CurrentPatyAlter != null)
                {
                    num = null;
                    value = "";
                    HIS_TREATMENT_TYPE hIS_TREATMENT_TYPE = ((rdo.TreatmentTypes != null) ? rdo.TreatmentTypes.FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == rdo.CurrentPatyAlter.TREATMENT_TYPE_ID) : null);
                    if (hIS_TREATMENT_TYPE != null)
                    {
                        try
                        {
                            num = (int)hIS_TREATMENT_TYPE.ID;
                            value = hIS_TREATMENT_TYPE.TREATMENT_TYPE_NAME.ToUpper();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        long tREATMENT_TYPE_ID = rdo.CurrentPatyAlter.TREATMENT_TYPE_ID;
                        long num2 = tREATMENT_TYPE_ID;
                        long num3 = num2 - 1;
                        if ((ulong)num3 > 5uL)
                        {
                            goto IL_01ac;
                        }
                        switch (num3)
                        {
                            case 0L:
                                break;
                            case 1L:
                                goto IL_0168;
                            case 2L:
                                goto IL_0179;
                            case 5L:
                                goto IL_018a;
                            case 4L:
                                goto IL_019b;
                            default:
                                goto IL_01ac;
                        }
                        num = 1;
                        value = "KHÁM BỆNH";
                    }
                    goto IL_01be;
                }
                goto IL_023a;
            IL_0168:
                num = 2;
                value = "ĐIỀU TRỊ NGOẠI TRÚ";
                goto IL_01be;
            IL_019b:
                num = 5;
                value = "ĐIỀU TRỊ LƯU TẠI TYT XÃ, PKĐKKV";
                goto IL_01be;
            IL_018a:
                num = 6;
                value = "NHẬN THUỐC THEO HẸN (KHÔNG KHÁM BỆNH)";
                goto IL_01be;
            IL_0179:
                num = 3;
                value = "ĐIỀU TRỊ NỘI TRÚ";
                goto IL_01be;
            IL_01ac:
                num = null;
                value = "(KHÔNG XÁC ĐỊNH ĐƯỢC ĐỐI TƯỢNG)";
                goto IL_01be;
            IL_01be:
                SetSingleKey(new KeyValue("TYPE_INDEX", num));
                SetSingleKey(new KeyValue("TYPE_NAME", value));
                if (rdo.CurrentPatyAlter.FREE_CO_PAID_TIME.HasValue)
                {
                    SetSingleKey(new KeyValue("FREE_CO_PAID_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.CurrentPatyAlter.FREE_CO_PAID_TIME.Value)));
                }
                goto IL_023a;
            IL_023a:

                if (patyAlterBHYTADOs != null && patyAlterBHYTADOs.Count > 0)
                {
                    string value2 = string.Join(";", (from o in patyAlterBHYTADOs.Select((PatyAlterBhytADO s) => s.HEIN_MEDI_ORG_CODE).Distinct()
                                                      orderby o
                                                      select o).ToList());
                    string value3 = string.Join(";", (from o in patyAlterBHYTADOs.Select((PatyAlterBhytADO s) => s.HEIN_MEDI_ORG_NAME).Distinct()
                                                      orderby o
                                                      select o).ToList());
                    SetSingleKey(new KeyValue("HEIN_MEDI_ORG_CODE", value2));
                    SetSingleKey(new KeyValue("HEIN_MEDI_ORG_NAME", value3));
                    string keyPaty = "";
                    HIS_SERE_SERV hIS_SERE_SERV = rdo.SereServs.OrderByDescending((HIS_SERE_SERV o) => o.ID).FirstOrDefault((HIS_SERE_SERV o) => !string.IsNullOrEmpty(o.HEIN_CARD_NUMBER) && o.IS_EXPEND != 1 && o.IS_NO_EXECUTE != 1);
                    if (hIS_SERE_SERV != null)
                    {
                        PatientTypeAlterProcessor.GetPatientTypeAlter(hIS_SERE_SERV, rdo.PatientTypeCFG, rdo.Treatment.TDL_TREATMENT_TYPE_ID.GetValueOrDefault(), ref keyPaty);
                    }
                    PatyAlterBhytADO patyAlterBhytADO = patyAlterBHYTADOs.OrderByDescending((PatyAlterBhytADO o) => o.LOG_TIME).FirstOrDefault((PatyAlterBhytADO o) => !string.IsNullOrEmpty(o.HEIN_CARD_NUMBER) && (string.IsNullOrWhiteSpace(keyPaty) || o.KEY == keyPaty));
                    if (patyAlterBhytADO != null)
                    {
                        HIS_BRANCH hIS_BRANCH = ((rdo.Branch != null) ? rdo.Branch : new HIS_BRANCH());
                        string text = ((!string.IsNullOrWhiteSpace(patyAlterBhytADO.HEIN_MEDI_ORG_CODE)) ? patyAlterBhytADO.HEIN_MEDI_ORG_CODE.Substring(0, 2) : "");
                        HIS_MEDI_ORG hIS_MEDI_ORG = ((rdo.ListMediOrg != null && rdo.ListMediOrg.Count > 0) ? rdo.ListMediOrg.FirstOrDefault((HIS_MEDI_ORG o) => o.MEDI_ORG_CODE == patyAlterBhytADO.HEIN_MEDI_ORG_CODE) : new HIS_MEDI_ORG());
                        SetSingleKey(new KeyValue("IS_HEIN", "X"));
                        string value4 = "";
                        string value5 = "";
                        string value6 = "";
                        string value7 = "";
                        if (patyAlterBhytADO.RIGHT_ROUTE_CODE == "DT")
                        {
                            if (patyAlterBhytADO.RIGHT_ROUTE_TYPE_CODE == "CC")
                            {
                                value4 = "X";
                            }
                            else if (patyAlterBhytADO.RIGHT_ROUTE_TYPE_CODE == "TH")
                            {
                                value7 = "X";
                            }
                            else if (patyAlterBhytADO.RIGHT_ROUTE_TYPE_CODE == "GT")
                            {
                                value5 = "X";
                            }
                            else if (patyAlterBhytADO.HEIN_MEDI_ORG_CODE == hIS_BRANCH.HEIN_MEDI_ORG_CODE || (!string.IsNullOrWhiteSpace(hIS_BRANCH.ACCEPT_HEIN_MEDI_ORG_CODE) && hIS_BRANCH.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(patyAlterBhytADO.HEIN_MEDI_ORG_CODE)))
                            {
                                value5 = "X";
                            }
                            else if (hIS_BRANCH.HEIN_LEVEL_CODE == "3" || hIS_BRANCH.HEIN_LEVEL_CODE == "4")
                            {
                                if (text == hIS_BRANCH.HEIN_PROVINCE_CODE && hIS_MEDI_ORG != null && (hIS_MEDI_ORG.LEVEL_CODE == "3" || hIS_MEDI_ORG.LEVEL_CODE == "4"))
                                {
                                    value7 = "X";
                                }
                                else
                                {
                                    value6 = "X";
                                }
                            }
                            else
                            {
                                value5 = "X";
                                if (rdo.Treatment.MEDI_ORG_CODE != rdo.CurrentPatyAlter.HEIN_MEDI_ORG_CODE)
                                {
                                    SetSingleKey(new KeyValue("THONG_TUYEN", "X"));
                                }
                            }
                        }
                        else if (patyAlterBhytADO.RIGHT_ROUTE_CODE == "TT")
                        {
                            value6 = "X";
                        }
                        SetSingleKey(new KeyValue("RIGHT_ROUTE_TYPE_NAME_CC", value4));
                        SetSingleKey(new KeyValue("RIGHT_ROUTE_TYPE_NAME", value5));
                        SetSingleKey(new KeyValue("NOT_RIGHT_ROUTE_TYPE_NAME", value6));
                        SetSingleKey(new KeyValue("RIGHT_ROUTE_TYPE_NAME_TT", value7));
                    }
                    else
                    {
                        SetSingleKey(new KeyValue("IS_NOT_HEIN", "X"));
                    }
                }
                else
                {
                    SetSingleKey(new KeyValue("IS_NOT_HEIN", "X"));
                }
                if (rdo.DepartmentTrans != null && rdo.DepartmentTrans.Count > 0)
                {
                    if (rdo.DepartmentTrans[0].DEPARTMENT_IN_TIME.HasValue)
                    {
                        SetSingleKey(new KeyValue("OPEN_TIME_SEPARATE_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.DepartmentTrans[0].DEPARTMENT_IN_TIME.GetValueOrDefault())));
                    }
                    if (rdo.DepartmentTrans[rdo.DepartmentTrans.Count - 1] != null && rdo.DepartmentTrans.Count > 1)
                    {
                        SetSingleKey(new KeyValue("DEPARTMENT_NAME_CLOSE_TREATMENT", rdo.DepartmentTrans[rdo.DepartmentTrans.Count - 1].DEPARTMENT_NAME));
                    }
                }
                if (!string.IsNullOrEmpty(rdo.SingleKeyValue.roomName))
                {
                    SetSingleKey(new KeyValue("ROOM_NAME", rdo.SingleKeyValue.roomName));
                }
                if (rdo.Treatment != null)
                {
                    SetSingleKey(new KeyValue("HEIN_CARD_ADDRESS", (object)rdo.Treatment.TDL_PATIENT_ADDRESS));
                    if (rdo.Treatment.CLINICAL_IN_TIME.HasValue)
                    {
                        SetSingleKey(new KeyValue("CLINICAL_IN_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.Treatment.CLINICAL_IN_TIME.Value)));
                    }
                    if (rdo.Treatment.OUT_TIME.HasValue)
                    {
                        SetSingleKey(new KeyValue("CLOSE_TIME_SEPARATE_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.Treatment.OUT_TIME.Value)));
                    }
                    if (rdo.Treatment.END_DEPARTMENT_ID.HasValue)
                    {
                        HIS_DEPARTMENT hIS_DEPARTMENT = rdo.Departments.FirstOrDefault((HIS_DEPARTMENT o) => o.ID == rdo.Treatment.END_DEPARTMENT_ID.Value);
                        if (hIS_DEPARTMENT != null)
                        {
                            SetSingleKey(new KeyValue("DEPARTMENT_BHYT_CODE", hIS_DEPARTMENT.BHYT_CODE));
                            SetSingleKey(new KeyValue("END_DEPARTMENT_CODE", hIS_DEPARTMENT.DEPARTMENT_CODE));
                            SetSingleKey(new KeyValue("END_DEPARTMENT_NAME", hIS_DEPARTMENT.DEPARTMENT_NAME));
                        }
                    }
                    int? num4 = null;
                    string value8;
                    switch (rdo.Treatment.TDL_PATIENT_GENDER_ID)
                    {
                        case 2L:
                            num4 = 1;
                            value8 = rdo.Treatment.TDL_PATIENT_GENDER_NAME;
                            break;
                        case 1L:
                            num4 = 2;
                            value8 = rdo.Treatment.TDL_PATIENT_GENDER_NAME;
                            break;
                        default:
                            num4 = 3;
                            value8 = "Không xác định";
                            break;
                    }
                    SetSingleKey(new KeyValue("GENDER_INDEX", num4));
                    SetSingleKey(new KeyValue("GENDER_NAME", value8));
                    SetSingleKey(new KeyValue("TRAN_PATI_MEDI_ORG_CODE", rdo.Treatment.TRANSFER_IN_MEDI_ORG_CODE));
                    SetSingleKey(new KeyValue("TRAN_PATI_MEDI_ORG_NAME", rdo.Treatment.TRANSFER_IN_MEDI_ORG_NAME));
                    if (rdo.Treatment.TREATMENT_RESULT_ID.HasValue || rdo.Treatment.TREATMENT_RESULT_ID.HasValue)
                    {
                        int num5 = 2;
                        if (rdo.Treatment.TREATMENT_RESULT_ID == 2 || rdo.Treatment.TREATMENT_RESULT_ID == 1 || rdo.Treatment.TREATMENT_END_TYPE_ID == 3 || rdo.Treatment.TREATMENT_END_TYPE_ID == 6 || rdo.Treatment.TREATMENT_END_TYPE_ID == 4)
                        {
                            num5 = 1;
                            if ((rdo.Treatment.TREATMENT_RESULT_ID == 2 || rdo.Treatment.TREATMENT_RESULT_ID == 1) && rdo.Treatment.TREATMENT_END_TYPE_ID == 2)
                            {
                                num5 = 2;
                            }
                        }
                        SetSingleKey(new KeyValue("TREATMENT_RESULT_INDEX", num5));
                    }
                    if (rdo.Treatment.OUT_TIME.HasValue)
                    {
                        long num6 = 0L;
                        num6 = ((!rdo.Treatment.CLINICAL_IN_TIME.HasValue) ? rdo.Treatment.IN_TIME : rdo.Treatment.CLINICAL_IN_TIME.Value);
                        DateTime? dateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(num6);
                        DateTime? dateTime2 = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(rdo.Treatment.OUT_TIME.Value);
                        if (dateTime.HasValue && dateTime2.HasValue)
                        {
                            TimeSpan timeSpan = dateTime2.Value - dateTime.Value;
                            if (timeSpan.Days > 1 || (timeSpan.Days == 1 && (timeSpan.Hours >= 1 || timeSpan.Minutes >= 1 || timeSpan.Seconds >= 1)))
                            {
                                if (rdo.Treatment.TDL_TREATMENT_TYPE_ID != 1)
                                {
                                    DateTime dateTime3 = new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day);
                                    DateTime dateTime4 = new DateTime(dateTime2.Value.Year, dateTime2.Value.Month, dateTime2.Value.Day);
                                    int num7 = (int)(dateTime4 - dateTime3).TotalDays + 1;
                                    SetSingleKey(new KeyValue("TREATMENT_DAY_COUNT_6556", num7));
                                }
                            }
                            else if (rdo.Treatment.TDL_TREATMENT_TYPE_ID == 3)
                            {
                                SetSingleKey(new KeyValue("TREATMENT_DAY_COUNT_6556", 1));
                            }
                        }
                    }
                    if (rdo.ListTransactionBill != null && rdo.ListTransactionBill.Count > 0 && rdo.Treatment.IS_PAUSE == 1)
                    {
                        HIS_TRANSACTION hIS_TRANSACTION = (from o in rdo.ListTransactionBill
                                                           where o.IS_DIRECTLY_BILLING != 1 && o.IS_CANCEL != 1
                                                           orderby o.TRANSACTION_TIME descending
                                                           select o).FirstOrDefault();
                        if (hIS_TRANSACTION != null)
                        {
                            if (hIS_TRANSACTION.TREATMENT_DEPOSIT_AMOUNT.HasValue)
                            {
                                SetSingleKey(new KeyValue("TRANSACTION_TREATMENT_DEPOSIT_AMOUNT", Inventec.Common.Number.Convert.NumberToStringRoundAuto(hIS_TRANSACTION.TREATMENT_DEPOSIT_AMOUNT.GetValueOrDefault(), 0)));
                            }
                            if (hIS_TRANSACTION.TREATMENT_REPAY_AMOUNT.HasValue)
                            {
                                SetSingleKey(new KeyValue("TRANSACTION_TREATMENT_REPAY_AMOUNT", Inventec.Common.Number.Convert.NumberToStringRoundAuto(hIS_TRANSACTION.TREATMENT_REPAY_AMOUNT.GetValueOrDefault(), 0)));
                            }
                            if (hIS_TRANSACTION.TREATMENT_PATIENT_PRICE.HasValue)
                            {
                                SetSingleKey(new KeyValue("TRANSACTION_TREATMENT_PATIENT_PRICE", Inventec.Common.Number.Convert.NumberToStringRoundAuto(hIS_TRANSACTION.TREATMENT_PATIENT_PRICE.GetValueOrDefault(), 0)));
                            }
                            if (hIS_TRANSACTION.TREATMENT_BILL_AMOUNT.HasValue)
                            {
                                SetSingleKey(new KeyValue("TRANSACTION_TREATMENT_BILL_AMOUNT", Inventec.Common.Number.Convert.NumberToStringRoundAuto(hIS_TRANSACTION.TREATMENT_BILL_AMOUNT.GetValueOrDefault(), 0)));
                            }
                            IEnumerable<HIS_TRANSACTION> enumerable = rdo.ListTransactionBill.Where((HIS_TRANSACTION o) => o.IS_DIRECTLY_BILLING != 1 && o.IS_CANCEL != 1 && !o.SALE_TYPE_ID.HasValue);
                            SetSingleKey(new KeyValue("LAST_TRANSACTION_AMOUNT", Inventec.Common.Number.Convert.NumberToStringRoundAuto((enumerable != null && enumerable.Count() > 0) ? enumerable.OrderByDescending((HIS_TRANSACTION o) => o.TRANSACTION_TIME).FirstOrDefault().AMOUNT : 0m, 0)));
                        }
                    }
                }
                if (rdo.Treatment.OUT_TIME.HasValue)
                {
                    long num6 = 0L;
                    num6 = ((!rdo.Treatment.CLINICAL_IN_TIME.HasValue) ? rdo.Treatment.IN_TIME : rdo.Treatment.CLINICAL_IN_TIME.Value);
                    DateTime? dateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(num6);
                    DateTime? dateTime2 = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(rdo.Treatment.OUT_TIME.Value);
                    if (dateTime.HasValue && dateTime2.HasValue)
                    {
                        TimeSpan timeSpan = dateTime2.Value - dateTime.Value;
                        if (timeSpan.Days > 1 || (timeSpan.Days == 1 && (timeSpan.Hours >= 1 || timeSpan.Minutes >= 1 || timeSpan.Seconds >= 1)))
                        {
                            if (rdo.Treatment.TDL_TREATMENT_TYPE_ID != 1)
                            {
                                DateTime dateTime3 = new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day);
                                DateTime dateTime4 = new DateTime(dateTime2.Value.Year, dateTime2.Value.Month, dateTime2.Value.Day);
                                int num7 = (int)(dateTime4 - dateTime3).TotalDays + 1;
                                SetSingleKey(new KeyValue("TOTAL_DAY", num7));
                            }
                        }
                        else if (rdo.Treatment.TDL_TREATMENT_TYPE_ID == 3)
                        {
                            SetSingleKey(new KeyValue("TOTAL_DAY", 1));
                        }
                    }
                }
                SetSingleKey(new KeyValue("CURRENT_DATE_SEPARATE_FULL_STR", rdo.SingleKeyValue.currentDateSeparateFullTime));
                string text2 = "";
                string value9 = "";
                string value10 = "";
                decimal num8 = default(decimal);
                decimal number = default(decimal);
                decimal num9 = default(decimal);
                decimal num10 = default(decimal);
                decimal num11 = default(decimal);
                long num12 = 0L;
                decimal number2 = default(decimal);
                decimal num13 = default(decimal);
                decimal number3 = default(decimal);
                decimal num14 = default(decimal);
                decimal number4 = default(decimal);
                decimal num15 = default(decimal);
                if (sereServADOs != null && sereServADOs.Count > 0)
                {
                    List<SereServADO> list = (from o in sereServADOs
                                              where o.HEIN_SERVICE_TYPE_ID == rdo.HeinServiceTypeCFG.HEIN_SERVICE_TYPE__EXAM_ID
                                              orderby o.CREATE_TIME
                                              select o).ToList();
                    if (list != null && list.Count > 0)
                    {
                        value9 = list[0].EXECUTE_ROOM_NAME;
                        value10 = list[list.Count - 1].EXECUTE_ROOM_NAME;
                        foreach (SereServADO item in list)
                        {
                            text2 = text2 + item.EXECUTE_ROOM_NAME + ", ";
                        }
                    }
                    number = sereServADOs.Sum((SereServADO o) => o.TOTAL_PRICE_BHYT);
                    num8 = sereServADOs.Sum((SereServADO o) => o.VIR_TOTAL_PRICE_NO_EXPEND.GetValueOrDefault());
                    num9 = sereServADOs.Sum((SereServADO o) => o.VIR_TOTAL_HEIN_PRICE).GetValueOrDefault();
                    num11 = sereServADOs.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE_BHYT).GetValueOrDefault();
                    num10 = sereServADOs.Sum((SereServADO o) => o.OTHER_SOURCE_PRICE).GetValueOrDefault();
                    number2 = sereServADOs.Sum((SereServADO o) => o.TOTAL_PRICE_PATIENT_SELF);
                    num14 = sereServADOs.Sum((SereServADO o) => o.TOTAL_PRICE_VP);
                    number3 = sereServADOs.Sum((SereServADO o) => o.TOTAL_PATIENT_PRICE_LEFT);
                    num12 = (long)rdo.SereServs.Where(p => rdo.materialTypes != null && rdo.materialTypes.Where(q => q.IS_FILM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(r => r.SERVICE_ID).Contains(p.SERVICE_ID)).Sum((o) => (decimal)o.AMOUNT);
                    if (num12 > 0)
                    {
                        SetSingleKey(new KeyValue("TOTAL_NUMBER_FILM", num12));
                        SetSingleKey(new KeyValue("TOTAL_NUMBER_FILM_STR", string.Format("Bệnh nhân đã nhận đủ số phim . Số phim {0}", num12)));
                    }
                    num13 = sereServADOs.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE.GetValueOrDefault());
                }
                if (sereServADONoExamZero != null && sereServADONoExamZero.Count > 0)
                {
                    number4 = sereServADONoExamZero.Sum((SereServADO o) => o.TOTAL_PRICE_PATIENT_SELF);
                    num15 = sereServADONoExamZero.Sum((SereServADO o) => o.VIR_TOTAL_PRICE_NO_EXPEND.GetValueOrDefault());
                }
                SetSingleKey(new KeyValue("EXECUTE_ROOM_EXAM", text2));
                SetSingleKey(new KeyValue("FIRST_EXAM_ROOM_NAME", value9));
                SetSingleKey(new KeyValue("LAST_EXAM_ROOM_NAME", value10));
                if (!string.IsNullOrEmpty(rdo.SingleKeyValue.departmentName))
                {
                    SetSingleKey(new KeyValue("DEPARTMENT_NAME", rdo.SingleKeyValue.departmentName));
                }
                SetSingleKey(new KeyValue("TOTAL_PRICE", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num8, 0)));
                SetSingleKey(new KeyValue("TOTAL_PRICE_BHYT", Inventec.Common.Number.Convert.NumberToStringRoundAuto(number, 0)));
                SetSingleKey(new KeyValue("TOTAL_PRICE_HEIN", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num9, 0)));
                SetSingleKey(new KeyValue("TOTAL_PRICE_PATIENT", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num11, 0)));
                SetSingleKey(new KeyValue("TOTAL_PRICE_PATIENT_SELF", Inventec.Common.Number.Convert.NumberToStringRoundAuto(number2, 0)));
                SetSingleKey(new KeyValue("TOTAL_PRICE_OTHER", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num10, 0)));
                SetSingleKey(new KeyValue("TOTAL_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(num8).ToString())));
                SetSingleKey(new KeyValue("TOTAL_PRICE_HEIN_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(num9).ToString())));
                SetSingleKey(new KeyValue("TOTAL_PRICE_PATIENT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(num11).ToString())));
                SetSingleKey(new KeyValue("TOTAL_PRICE_OTHER_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(num10).ToString())));
                SetSingleKey(new KeyValue("TOTAL_PRICE_PATIENT_VIR", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num13, 0)));
                SetSingleKey(new KeyValue("TOTAL_PRICE_PATIENT_VIR_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(num13).ToString())));
                SetSingleKey(new KeyValue("TOTAL_PRICE_NEW", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num14, 0)));
                SetSingleKey(new KeyValue("TOTAL_PATIENT_PRICE_LEFT", Inventec.Common.Number.Convert.NumberToStringRoundAuto(number3, 0)));
                SetSingleKey(new KeyValue("TOTAL_PRICE_NEW_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(num14).ToString())));
                SetSingleKey(new KeyValue("TOTAL_PRICE_NO_EXAM_ZERO", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num15, 0)));
                SetSingleKey(new KeyValue("TOTAL_PRICE_NO_EXAM_ZERO_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(num15).ToString())));
                SetSingleKey(new KeyValue("TOTAL_PATIENT_PRICE_LEFT_NO_EXAM_ZERO", Inventec.Common.Number.Convert.NumberToStringRoundAuto(number4, 0)));
                decimal num16 = default(decimal);
                decimal num17 = default(decimal);
                if (rdo.ListSereServDeposits != null && rdo.ListSereServDeposits.Count > 0)
                {
                    num16 = rdo.ListSereServDeposits.Where((HIS_SERE_SERV_DEPOSIT o) => o.IS_CANCEL != 1).Sum((HIS_SERE_SERV_DEPOSIT s) => s.AMOUNT);
                }
                if (rdo.TreatmentFees != null)
                {
                    SetSingleKey(new KeyValue("TOTAL_DEPOSIT_AMOUNT", Inventec.Common.Number.Convert.NumberToStringRoundAuto(rdo.TreatmentFees[0].TOTAL_DEPOSIT_AMOUNT.GetValueOrDefault(), 0)));
                    decimal num18 = default(decimal);
                    decimal num19 = default(decimal);
                    decimal num20 = default(decimal);
                    decimal num21 = default(decimal);
                    decimal num22 = default(decimal);
                    decimal num23 = default(decimal);
                    decimal num24 = default(decimal);
                    decimal num25 = default(decimal);
                    decimal num26 = default(decimal);
                    decimal num27 = default(decimal);
                    num18 = rdo.TreatmentFees[0].TOTAL_PRICE.GetValueOrDefault();
                    num19 = rdo.TreatmentFees[0].TOTAL_HEIN_PRICE.GetValueOrDefault();
                    num20 = rdo.TreatmentFees[0].TOTAL_PATIENT_PRICE.GetValueOrDefault();
                    num21 = rdo.TreatmentFees[0].TOTAL_DEPOSIT_AMOUNT.GetValueOrDefault();
                    num22 = rdo.TreatmentFees[0].TOTAL_BILL_AMOUNT.GetValueOrDefault();
                    num23 = rdo.TreatmentFees[0].TOTAL_BILL_TRANSFER_AMOUNT.GetValueOrDefault();
                    num25 = default(decimal);
                    num24 = rdo.TreatmentFees[0].TOTAL_REPAY_AMOUNT.GetValueOrDefault();
                    num27 = num21 + num22 - num23 - num24 + num25;
                    decimal num28 = num20 - num27;
                    num26 = num28;
                    SetSingleKey(new KeyValue("TREATMENT_FEE_TOTAL_PRICE", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num18, 0)));
                    SetSingleKey(new KeyValue("TREATMENT_FEE_TOTAL_PATIENT_PRICE", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num20, 0)));
                    SetSingleKey(new KeyValue("TREATMENT_FEE_TOTAL_OBTAINED_PRICE", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num27, 0)));
                    SetSingleKey(new KeyValue("TREATMENT_FEE_TRANSFER", Inventec.Common.Number.Convert.NumberToStringRoundAuto(num28, 0)));
                    SetSingleKey(new KeyValue("TREATMENT_CAN_THU", num28));
                    AddObjectKeyIntoListkey(rdo.TreatmentFees[0], false);
                    num17 = rdo.TreatmentFees[0].TOTAL_DEPOSIT_AMOUNT.GetValueOrDefault() - num16;
                }
                else
                {
                    SetSingleKey(new KeyValue("TOTAL_DEPOSIT_AMOUNT", 0));
                }
                SetSingleKey(new KeyValue("TOTAL_DEPOSIT_SERVICE_AMOUNT", num16));
                SetSingleKey(new KeyValue("TOTAL_DEPOSIT_OTHER_AMOUNT", num17));
                SetSingleKey(new KeyValue("CREATOR_USERNAME", ClientTokenManagerStore.ClientTokenManager.GetUserName()));
                AddObjectKeyIntoListkey(patientADO);
                AddObjectKeyIntoListkey(rdo.Treatment, false);
                if (rdo.CurrentPatyAlter != null)
                {
                    AddObjectKeyIntoListkey(DataRawProcess.PatyAlterBHYTRawToADO(rdo.CurrentPatyAlter, rdo.Branch, rdo.TreatmentTypes), false);
                    if (rdo.CurrentPatyAlter.HEIN_CARD_FROM_TIME.HasValue)
                    {
                        SetSingleKey(new KeyValue("STR_HEIN_CARD_FROM_TIME", Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.CurrentPatyAlter.HEIN_CARD_FROM_TIME.GetValueOrDefault())));
                    }
                    if (rdo.CurrentPatyAlter.HEIN_CARD_TO_TIME.HasValue)
                    {
                        SetSingleKey(new KeyValue("STR_HEIN_CARD_TO_TIME", Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.CurrentPatyAlter.HEIN_CARD_TO_TIME.GetValueOrDefault())));
                    }
                    if (rdo.CurrentPatyAlter.JOIN_5_YEAR_TIME.HasValue)
                    {
                        SetSingleKey(new KeyValue("JOIN_5_YEAR_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.CurrentPatyAlter.JOIN_5_YEAR_TIME.GetValueOrDefault())));
                    }
                    SetSingleKey(new KeyValue("RATIO_STR", DataRawProcess.GetDefaultHeinRatioForView(rdo.CurrentPatyAlter.HEIN_CARD_NUMBER, rdo.CurrentPatyAlter.HEIN_TREATMENT_TYPE_CODE, rdo.Branch.HEIN_LEVEL_CODE, rdo.CurrentPatyAlter.RIGHT_ROUTE_CODE)));
                    SetSingleKey(new KeyValue("LIVE_AREA_CODE", rdo.CurrentPatyAlter.LIVE_AREA_CODE));
                }
            }
            catch (Exception ex2)
            {
                LogSystem.Error(ex2);
            }
        }

        private void SetQrCode()
        {
            try
            {
                if (rdo.TransReq == null || rdo.ListHisConfigPaymentQrCode == null || rdo.ListHisConfigPaymentQrCode.Count <= 0)
                {
                    return;
                }
                Dictionary<string, object> dictionary = QrCodeProcessor.CreateQrImage(rdo.TransReq, rdo.ListHisConfigPaymentQrCode);
                if (dictionary == null || dictionary.Count <= 0)
                {
                    return;
                }
                foreach (KeyValuePair<string, object> item in dictionary)
                {
                    SetSingleKey(new KeyValue(item.Key, item.Value));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void DataInputProcess()
        {
            try
            {
                patientADO = DataRawProcess.PatientRawToADO(rdo.Treatment);
                List<HIS_PATIENT_TYPE_ALTER> ListPta = rdo.PatientTypeAlterAlls.OrderByDescending((HIS_PATIENT_TYPE_ALTER o) => o.LOG_TIME).ToList();
                List<Task> list = new List<Task>();
                Task item = Task.Factory.StartNew(delegate
                {
                    sereServADOs = new List<SereServADO>();
                    List<SereServADO> list2 = new List<SereServADO>();
                    list2.AddRange(rdo.SereServs.Select((HIS_SERE_SERV r) => new SereServADO(r, rdo.SereServs, rdo.SereServExts, rdo.HeinServiceTypes, rdo.Services, rdo.Rooms, rdo.medicineTypes, rdo.MedicineLines, rdo.materialTypes, rdo.PatientTypeCFG, rdo.HisConfigValue, rdo.HisServiceUnit, rdo.Treatment, ListPta, rdo.ListPatientType, false, rdo.ListSereServBills, rdo.ListSereServDeposits, rdo.ListSeseDepoRepays)));
                    var list3 = (from o in list2
                                 where o.AMOUNT > 0m && o.IS_NO_EXECUTE != 1 && (!rdo.HisConfigValue.IsNotIncludeIsExpend || (rdo.HisConfigValue.IsNotIncludeIsExpend && o.IS_EXPEND != 1))
                                 orderby o.HEIN_SERVICE_TYPE_NUM_ORDER ?? 99999
                                 group o by new { o.SERVICE_ID, o.PRIMARY_PRICE, o.PRICE_BHYT, o.SERVICE_PAY_RATE, o.BHYT_PAY_RATE, o.IS_EXPEND, o.NUMBER_OF_FILM, o.KEY_PATY_ALTER, o.HEIN_SERVICE_TYPE_ID, o.STENT_ORDER, o.GROUP_DEPARTMENT_ID }).ToList();
                    foreach (var item4 in list3)
                    {
                        SereServADO sereServADO = item4.FirstOrDefault();
                        sereServADO.AMOUNT = item4.Sum((SereServADO o) => o.AMOUNT);
                        sereServADO.VIR_TOTAL_HEIN_PRICE = item4.Sum((SereServADO o) => o.VIR_TOTAL_HEIN_PRICE);
                        sereServADO.VIR_TOTAL_PATIENT_PRICE_BHYT = item4.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE_BHYT);
                        sereServADO.TOTAL_PRICE_BHYT = item4.Sum((SereServADO o) => o.TOTAL_PRICE_BHYT);
                        sereServADO.VIR_TOTAL_PATIENT_PRICE = item4.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE);
                        sereServADO.VIR_TOTAL_PRICE_NO_EXPEND = item4.Sum((SereServADO o) => o.VIR_TOTAL_PRICE_NO_EXPEND);
                        sereServADO.TOTAL_PRICE_PATIENT_SELF = item4.Sum((SereServADO o) => o.TOTAL_PRICE_PATIENT_SELF);
                        sereServADO.OTHER_SOURCE_PRICE = item4.Sum((SereServADO o) => o.OTHER_SOURCE_PRICE);
                        sereServADO.TOTAL_PATIENT_PRICE_LEFT = item4.Sum((SereServADO o) => o.TOTAL_PATIENT_PRICE_LEFT);
                        sereServADO.TOTAL_PRICE_VP = item4.Sum((SereServADO o) => o.TOTAL_PRICE_VP);
                        sereServADO.IS_PAID = item4.Min((SereServADO o) => o.IS_PAID);
                        sereServADOs.Add(sereServADO);
                        if (sereServADO.STENT_ORDER.HasValue && sereServADO.STENT_ORDER.Value > 1)
                        {
                            decimal valueOrDefault = sereServADO.VIR_TOTAL_HEIN_PRICE.GetValueOrDefault();
                            decimal valueOrDefault2 = sereServADO.VIR_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault();
                            decimal valueOrDefault3 = sereServADO.OTHER_SOURCE_PRICE.GetValueOrDefault();
                            decimal num = ((valueOrDefault2 > 0m) ? valueOrDefault2 : valueOrDefault3);
                            sereServADO.TOTAL_PRICE_BHYT = valueOrDefault + num;
                        }
                    }
                    sereServADOs = (from o in sereServADOs
                                    orderby o.STENT_ORDER.GetValueOrDefault(), o.SERVICE_NAME
                                    select o).ToList();
                });
                list.Add(item);
                Task item2 = Task.Factory.StartNew(delegate
                {
                    List<SereServADO> list2 = new List<SereServADO>();
                    list2.AddRange(rdo.SereServs.Select((HIS_SERE_SERV r) => new SereServADO(r, rdo.SereServs, rdo.SereServExts, rdo.HeinServiceTypes, rdo.Services, rdo.Rooms, rdo.medicineTypes, rdo.MedicineLines, rdo.materialTypes, rdo.PatientTypeCFG, rdo.HisConfigValue, rdo.HisServiceUnit, rdo.Treatment, ListPta, rdo.ListPatientType, true, rdo.ListSereServBills, rdo.ListSereServDeposits, rdo.ListSeseDepoRepays)));
                    sereServADOSAs = new List<SereServADO>();
                    var list3 = (from o in list2
                                 where o.AMOUNT > 0m && o.IS_NO_EXECUTE != 1 && (!rdo.HisConfigValue.IsNotIncludeIsExpend || (rdo.HisConfigValue.IsNotIncludeIsExpend && o.IS_EXPEND != 1)) && !o.IsHide
                                 orderby o.HEIN_SERVICE_TYPE_NUM_ORDER ?? 99999
                                 group o by new { o.SERVICE_ID, o.PRIMARY_PRICE, o.PRICE_BHYT, o.SERVICE_PAY_RATE, o.BHYT_PAY_RATE, o.IS_EXPEND, o.NUMBER_OF_FILM, o.KEY_PATY_ALTER, o.HEIN_SERVICE_TYPE_ID, o.STENT_ORDER, o.GROUP_DEPARTMENT_ID }).ToList();
                    foreach (var item5 in list3)
                    {
                        SereServADO sereServADO = item5.FirstOrDefault();
                        sereServADO.AMOUNT = item5.Sum((SereServADO o) => o.AMOUNT);
                        sereServADO.VIR_TOTAL_HEIN_PRICE = item5.Sum((SereServADO o) => o.VIR_TOTAL_HEIN_PRICE);
                        sereServADO.VIR_TOTAL_PATIENT_PRICE_BHYT = item5.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE_BHYT);
                        sereServADO.TOTAL_PRICE_BHYT = item5.Sum((SereServADO o) => o.TOTAL_PRICE_BHYT);
                        sereServADO.VIR_TOTAL_PATIENT_PRICE = item5.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE);
                        sereServADO.VIR_TOTAL_PRICE_NO_EXPEND = item5.Sum((SereServADO o) => o.VIR_TOTAL_PRICE_NO_EXPEND);
                        sereServADO.TOTAL_PRICE_PATIENT_SELF = item5.Sum((SereServADO o) => o.TOTAL_PRICE_PATIENT_SELF);
                        sereServADO.OTHER_SOURCE_PRICE = item5.Sum((SereServADO o) => o.OTHER_SOURCE_PRICE);
                        sereServADO.TOTAL_PATIENT_PRICE_LEFT = item5.Sum((SereServADO o) => o.TOTAL_PATIENT_PRICE_LEFT);
                        sereServADO.TOTAL_PRICE_VP = item5.Sum((SereServADO o) => o.TOTAL_PRICE_VP);
                        sereServADO.IS_PAID = item5.Min((SereServADO o) => o.IS_PAID);
                        sereServADOSAs.Add(sereServADO);
                        if (sereServADO.STENT_ORDER.HasValue && sereServADO.STENT_ORDER.Value > 1)
                        {
                            decimal valueOrDefault = sereServADO.VIR_TOTAL_HEIN_PRICE.GetValueOrDefault();
                            decimal valueOrDefault2 = sereServADO.VIR_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault();
                            decimal valueOrDefault3 = sereServADO.OTHER_SOURCE_PRICE.GetValueOrDefault();
                            decimal num = ((valueOrDefault2 > 0m) ? valueOrDefault2 : valueOrDefault3);
                            sereServADO.TOTAL_PRICE_BHYT = valueOrDefault + num;
                        }
                    }
                    sereServADOSAs = (from o in sereServADOSAs
                                      orderby o.STENT_ORDER.GetValueOrDefault(), o.SERVICE_NAME
                                      select o).ToList();
                });
                list.Add(item2);
                Task item3 = Task.Factory.StartNew(delegate
                {
                    List<SereServADO> list2 = new List<SereServADO>();
                    list2.AddRange(rdo.SereServs.Select((HIS_SERE_SERV r) => new SereServADO(r, rdo.SereServs, rdo.SereServExts, rdo.HeinServiceTypes, rdo.Services, rdo.Rooms, rdo.medicineTypes, rdo.MedicineLines, rdo.materialTypes, rdo.PatientTypeCFG, rdo.HisConfigValue, rdo.HisServiceUnit, rdo.Treatment, ListPta, rdo.ListPatientType, false, rdo.ListSereServBills, rdo.ListSereServDeposits, rdo.ListSeseDepoRepays)));
                    sereServADONoExamZero = new List<SereServADO>();
                    var list3 = (from o in list2.Where(delegate(SereServADO o)
                        {
                            if (o.AMOUNT > 0m && o.IS_NO_EXECUTE != 1)
                            {
                                if (o.TDL_SERVICE_TYPE_ID != 1)
                                {
                                    goto IL_0087;
                                }
                                if (o.TDL_SERVICE_TYPE_ID == 1)
                                {
                                    decimal? vIR_PRICE = o.VIR_PRICE;
                                    if ((vIR_PRICE.GetValueOrDefault() > default(decimal)) & vIR_PRICE.HasValue)
                                    {
                                        goto IL_0087;
                                    }
                                }
                            }
                            int result = 0;
                            goto IL_00ea;
                        IL_0087:
                            result = ((!rdo.HisConfigValue.IsNotIncludeIsExpend || (rdo.HisConfigValue.IsNotIncludeIsExpend && o.IS_EXPEND != 1)) ? 1 : 0);
                            goto IL_00ea;
                        IL_00ea:
                            return (byte)result != 0;
                        })
                                 orderby o.HEIN_SERVICE_TYPE_NUM_ORDER ?? 99999
                                 group o by new { o.SERVICE_ID, o.PRIMARY_PRICE, o.PRICE_BHYT, o.SERVICE_PAY_RATE, o.BHYT_PAY_RATE, o.IS_EXPEND, o.NUMBER_OF_FILM, o.KEY_PATY_ALTER, o.HEIN_SERVICE_TYPE_ID, o.STENT_ORDER, o.GROUP_DEPARTMENT_ID }).ToList();
                    foreach (var item6 in list3)
                    {
                        SereServADO sereServADO = item6.FirstOrDefault();
                        sereServADO.AMOUNT = item6.Sum((SereServADO o) => o.AMOUNT);
                        sereServADO.VIR_TOTAL_HEIN_PRICE = item6.Sum((SereServADO o) => o.VIR_TOTAL_HEIN_PRICE);
                        sereServADO.VIR_TOTAL_PATIENT_PRICE_BHYT = item6.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE_BHYT);
                        sereServADO.TOTAL_PRICE_BHYT = item6.Sum((SereServADO o) => o.TOTAL_PRICE_BHYT);
                        sereServADO.VIR_TOTAL_PATIENT_PRICE = item6.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE);
                        sereServADO.VIR_TOTAL_PRICE_NO_EXPEND = item6.Sum((SereServADO o) => o.VIR_TOTAL_PRICE_NO_EXPEND);
                        sereServADO.TOTAL_PRICE_PATIENT_SELF = item6.Sum((SereServADO o) => o.TOTAL_PRICE_PATIENT_SELF);
                        sereServADO.OTHER_SOURCE_PRICE = item6.Sum((SereServADO o) => o.OTHER_SOURCE_PRICE);
                        sereServADO.TOTAL_PATIENT_PRICE_LEFT = item6.Sum((SereServADO o) => o.TOTAL_PATIENT_PRICE_LEFT);
                        sereServADO.TOTAL_PRICE_VP = item6.Sum((SereServADO o) => o.TOTAL_PRICE_VP);
                        sereServADO.IS_PAID = item6.Min((SereServADO o) => o.IS_PAID);
                        sereServADONoExamZero.Add(sereServADO);
                        if (sereServADO.STENT_ORDER.HasValue && sereServADO.STENT_ORDER.Value > 1)
                        {
                            decimal valueOrDefault = sereServADO.VIR_TOTAL_HEIN_PRICE.GetValueOrDefault();
                            decimal valueOrDefault2 = sereServADO.VIR_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault();
                            decimal valueOrDefault3 = sereServADO.OTHER_SOURCE_PRICE.GetValueOrDefault();
                            decimal num = ((valueOrDefault2 > 0m) ? valueOrDefault2 : valueOrDefault3);
                            sereServADO.TOTAL_PRICE_BHYT = valueOrDefault + num;
                        }
                    }
                    sereServADONoExamZero = (from o in sereServADONoExamZero
                                             orderby o.STENT_ORDER.GetValueOrDefault(), o.SERVICE_NAME
                                             select o).ToList();
                });
                list.Add(item3);
                Task.WaitAll(list.ToArray());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GroupDisplayProcess()
        {
            try
            {
                patyAlterBHYTADOs = PatyAlterProcess(sereServADOs);
                heinServiceTypeADOs = HeinServiceTypeProcess(sereServADOs);
                sereServADOs.ForEach(delegate(SereServADO o)
                {
                    if (o.HEIN_SERVICE_TYPE_ID == 3 || o.HEIN_SERVICE_TYPE_ID == 4 || o.HEIN_SERVICE_TYPE_ID == 19 || o.HEIN_SERVICE_TYPE_ID == 20)
                    {
                        long? hEIN_SERVICE_TYPE_ID = o.HEIN_SERVICE_TYPE_ID;
                        o.HEIN_SERVICE_TYPE_PARENT_1_ID = hEIN_SERVICE_TYPE_ID;
                        o.HEIN_SERVICE_TYPE_ID = 125L;
                    }
                });
                sereServADOSAs.ForEach(delegate(SereServADO o)
                {
                    if (o.HEIN_SERVICE_TYPE_ID == 3 || o.HEIN_SERVICE_TYPE_ID == 4 || o.HEIN_SERVICE_TYPE_ID == 19 || o.HEIN_SERVICE_TYPE_ID == 20)
                    {
                        long? hEIN_SERVICE_TYPE_ID = o.HEIN_SERVICE_TYPE_ID;
                        o.HEIN_SERVICE_TYPE_PARENT_1_ID = hEIN_SERVICE_TYPE_ID;
                        o.HEIN_SERVICE_TYPE_ID = 125L;
                    }
                });
                medicineLineADOs = MedicineLineProcesss(sereServADOs);
                HeinServiceTypeBeds = HeinServiceTypeBedProcess(sereServADOs);
                GroupDepartmentProcess();
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void GroupDisplayProcess(List<SereServADO> sereServAdos, ref List<PatyAlterBhytADO> patyAlterBHYTADOs, ref List<HeinServiceTypeADO> heinServiceTypeADOs, ref List<HeinServiceTypeADO> HeinServiceTypeBeds, ref List<MedicineLineADO> medicineLineADOs)
        {
            try
            {
                patyAlterBHYTADOs = PatyAlterProcess(sereServAdos);
                heinServiceTypeADOs = HeinServiceTypeProcess(sereServAdos);
                sereServAdos.ForEach(delegate(SereServADO o)
                {
                    if (o.HEIN_SERVICE_TYPE_ID == 3 || o.HEIN_SERVICE_TYPE_ID == 4 || o.HEIN_SERVICE_TYPE_ID == 19 || o.HEIN_SERVICE_TYPE_ID == 20)
                    {
                        long? hEIN_SERVICE_TYPE_ID = o.HEIN_SERVICE_TYPE_ID;
                        o.HEIN_SERVICE_TYPE_PARENT_1_ID = hEIN_SERVICE_TYPE_ID;
                        o.HEIN_SERVICE_TYPE_ID = 125L;
                    }
                });
                HeinServiceTypeBeds = HeinServiceTypeBedProcess(sereServAdos);
                medicineLineADOs = MedicineLineProcesss(sereServAdos);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private List<HeinServiceTypeADO> HeinServiceTypeProcess(List<SereServADO> sereServAdos)
        {
            List<HeinServiceTypeADO> list = new List<HeinServiceTypeADO>();
            try
            {
                var list2 = (from o in sereServAdos
                             orderby o.HEIN_SERVICE_TYPE_NUM_ORDER ?? 99999999, o.TDL_INTRUCTION_TIME
                             group o by new { o.HEIN_SERVICE_TYPE_ID, o.KEY_PATY_ALTER, o.GROUP_DEPARTMENT_ID }).ToList();
                List<long> list3 = (from p in sereServAdos
                                    where (p.HEIN_SERVICE_TYPE_ID ?? 99999999) == p.PARENT_ID
                                    select p.PARENT_ID.GetValueOrDefault()).Distinct().ToList();
                int num = 1;
                foreach (var item in list2)
                {
                    HeinServiceTypeADO heinServiceType = new HeinServiceTypeADO();
                    SereServADO sereServBHYT = item.FirstOrDefault();
                    heinServiceType.KEY_PATY_ALTER = sereServBHYT.KEY_PATY_ALTER;
                    heinServiceType.GROUP_DEPARTMENT_ID = sereServBHYT.GROUP_DEPARTMENT_ID;
                    heinServiceType.TOTAL_PRICE_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.VIR_TOTAL_PRICE_NO_EXPEND.GetValueOrDefault());
                    heinServiceType.TOTAL_PRICE_BHYT_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.TOTAL_PRICE_BHYT);
                    heinServiceType.TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.VIR_TOTAL_HEIN_PRICE.GetValueOrDefault());
                    heinServiceType.TOTAL_PATIENT_PRICE_VIR_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE.GetValueOrDefault());
                    heinServiceType.TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault());
                    heinServiceType.TOTAL_PATIENT_PRICE_SELF_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.TOTAL_PRICE_PATIENT_SELF);
                    heinServiceType.OTHER_SOURCE_PRICE = item.Sum((SereServADO o) => o.OTHER_SOURCE_PRICE.GetValueOrDefault());
                    heinServiceType.TOTAL_PATIENT_PRICE_LEFT = item.Sum((SereServADO o) => o.TOTAL_PATIENT_PRICE_LEFT);
                    heinServiceType.TOTAL_PRICE_VP = item.Sum((SereServADO o) => o.TOTAL_PRICE_VP);
                    heinServiceType.TOTAL_BHYT_PRICE = heinServiceType.TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE.GetValueOrDefault() + heinServiceType.TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE.GetValueOrDefault();
                    heinServiceType.TOTAL_PRICE = heinServiceType.TOTAL_PRICE_HEIN_SERVICE_TYPE;
                    heinServiceType.TOTAL_HEIN_PRICE = heinServiceType.TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE;
                    heinServiceType.TOTAL_PATIENT_PRICE_SELF = heinServiceType.TOTAL_PATIENT_PRICE_SELF_HEIN_SERVICE_TYPE;
                    if (sereServBHYT.HEIN_SERVICE_TYPE_ID.HasValue)
                    {
                        if (list3 != null && list3.Count > 0 && list3.Contains(sereServBHYT.HEIN_SERVICE_TYPE_ID.Value))
                        {
                            HeinServiceTypeADO heinServiceTypeADO = list.FirstOrDefault((HeinServiceTypeADO o) => o.KEY_PATY_ALTER == heinServiceType.KEY_PATY_ALTER && o.ID == 126 && o.GROUP_DEPARTMENT_ID == heinServiceType.GROUP_DEPARTMENT_ID);
                            if (heinServiceTypeADO != null)
                            {
                                HeinServiceTypeADO heinServiceTypeADO2 = heinServiceTypeADO;
                                heinServiceTypeADO2.TOTAL_PRICE_HEIN_SERVICE_TYPE += heinServiceType.TOTAL_PRICE_HEIN_SERVICE_TYPE;
                                HeinServiceTypeADO heinServiceTypeADO3 = heinServiceTypeADO;
                                heinServiceTypeADO3.TOTAL_PRICE_BHYT_HEIN_SERVICE_TYPE += heinServiceType.TOTAL_PRICE_BHYT_HEIN_SERVICE_TYPE;
                                HeinServiceTypeADO heinServiceTypeADO4 = heinServiceTypeADO;
                                heinServiceTypeADO4.TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE += heinServiceType.TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE;
                                HeinServiceTypeADO heinServiceTypeADO5 = heinServiceTypeADO;
                                heinServiceTypeADO5.TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE += heinServiceType.TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE;
                                HeinServiceTypeADO heinServiceTypeADO6 = heinServiceTypeADO;
                                heinServiceTypeADO6.TOTAL_PATIENT_PRICE_SELF_HEIN_SERVICE_TYPE += heinServiceType.TOTAL_PATIENT_PRICE_SELF_HEIN_SERVICE_TYPE;
                                HeinServiceTypeADO heinServiceTypeADO7 = heinServiceTypeADO;
                                heinServiceTypeADO7.OTHER_SOURCE_PRICE += heinServiceType.OTHER_SOURCE_PRICE;
                                HeinServiceTypeADO heinServiceTypeADO8 = heinServiceTypeADO;
                                heinServiceTypeADO8.TOTAL_BHYT_PRICE += heinServiceType.TOTAL_BHYT_PRICE;
                                HeinServiceTypeADO heinServiceTypeADO9 = heinServiceTypeADO;
                                heinServiceTypeADO9.TOTAL_PRICE += heinServiceType.TOTAL_PRICE;
                                HeinServiceTypeADO heinServiceTypeADO10 = heinServiceTypeADO;
                                heinServiceTypeADO10.TOTAL_HEIN_PRICE += heinServiceType.TOTAL_HEIN_PRICE;
                                HeinServiceTypeADO heinServiceTypeADO11 = heinServiceTypeADO;
                                heinServiceTypeADO11.TOTAL_PATIENT_PRICE_SELF += heinServiceType.TOTAL_PATIENT_PRICE_SELF;
                                heinServiceTypeADO.TOTAL_PATIENT_PRICE_LEFT += heinServiceType.TOTAL_PATIENT_PRICE_LEFT;
                                heinServiceTypeADO.TOTAL_PRICE_VP += heinServiceType.TOTAL_PRICE_VP;
                            }
                            else
                            {
                                heinServiceTypeADO = new HeinServiceTypeADO();
                                heinServiceTypeADO.KEY_PATY_ALTER = sereServBHYT.KEY_PATY_ALTER;
                                heinServiceTypeADO.ID = 126L;
                                heinServiceTypeADO.GROUP_DEPARTMENT_ID = sereServBHYT.GROUP_DEPARTMENT_ID;
                                heinServiceTypeADO.HEIN_SERVICE_TYPE_NAME = "Gói vật tư y tế";
                                heinServiceTypeADO.NUM_ORDER = sereServBHYT.HEIN_SERVICE_TYPE_NUM_ORDER;
                                heinServiceTypeADO.TOTAL_PRICE_HEIN_SERVICE_TYPE = heinServiceType.TOTAL_PRICE_HEIN_SERVICE_TYPE;
                                heinServiceTypeADO.TOTAL_PRICE_BHYT_HEIN_SERVICE_TYPE = heinServiceType.TOTAL_PRICE_BHYT_HEIN_SERVICE_TYPE;
                                heinServiceTypeADO.TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE = heinServiceType.TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE;
                                heinServiceTypeADO.TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE = heinServiceType.TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE;
                                heinServiceTypeADO.TOTAL_PATIENT_PRICE_SELF_HEIN_SERVICE_TYPE = heinServiceType.TOTAL_PATIENT_PRICE_SELF_HEIN_SERVICE_TYPE;
                                heinServiceTypeADO.OTHER_SOURCE_PRICE = heinServiceType.OTHER_SOURCE_PRICE;
                                heinServiceTypeADO.TOTAL_BHYT_PRICE = heinServiceType.TOTAL_BHYT_PRICE;
                                heinServiceTypeADO.TOTAL_PRICE = heinServiceType.TOTAL_PRICE;
                                heinServiceTypeADO.TOTAL_HEIN_PRICE = heinServiceType.TOTAL_HEIN_PRICE;
                                heinServiceTypeADO.TOTAL_PATIENT_PRICE_SELF = heinServiceType.TOTAL_PATIENT_PRICE_SELF;
                                heinServiceTypeADO.TOTAL_PATIENT_PRICE_LEFT = heinServiceType.TOTAL_PATIENT_PRICE_LEFT;
                                heinServiceTypeADO.TOTAL_PRICE_VP = heinServiceType.TOTAL_PRICE_VP;
                                list.Add(heinServiceTypeADO);
                            }
                            List<SereServADO> list4 = item.Where((SereServADO o) => !o.STENT_ORDER.HasValue).ToList();
                            SereServADO sereServADO = (from o in item
                                                       where o.STENT_ORDER.HasValue
                                                       orderby o.STENT_ORDER
                                                       select o).FirstOrDefault();
                            if (sereServADO != null)
                            {
                                list4.Add(sereServADO);
                            }
                            heinServiceType.TOTAL_PRICE = list4.Sum((SereServADO s) => s.VIR_TOTAL_PRICE_NO_EXPEND.GetValueOrDefault());
                            heinServiceType.TOTAL_HEIN_PRICE = list4.Sum((SereServADO s) => s.VIR_TOTAL_HEIN_PRICE.GetValueOrDefault());
                            heinServiceType.TOTAL_BHYT_PRICE = heinServiceType.TOTAL_HEIN_PRICE + heinServiceType.TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE;
                            heinServiceType.TOTAL_PATIENT_PRICE_SELF = list4.Sum((SereServADO o) => o.TOTAL_PRICE_PATIENT_SELF);
                            heinServiceType.HEIN_SERVICE_TYPE_CHILD_NUM_ORDER = num;
                            heinServiceType.TOTAL_PRICE_VP = list4.Sum((SereServADO s) => s.TOTAL_PRICE_VP);
                            heinServiceType.TOTAL_PATIENT_PRICE_LEFT = list4.Sum((SereServADO s) => s.TOTAL_PATIENT_PRICE_LEFT);
                            HIS_SERE_SERV hIS_SERE_SERV = rdo.SereServs.FirstOrDefault((HIS_SERE_SERV o) => o.ID == sereServBHYT.HEIN_SERVICE_TYPE_ID.Value);
                            string hEIN_SERVICE_TYPE_NAME = string.Format("{0} {1}({2})", sereServBHYT.HEIN_SERVICE_TYPE_NAME, num, (hIS_SERE_SERV != null) ? hIS_SERE_SERV.TDL_HEIN_SERVICE_BHYT_NAME : null);
                            heinServiceType.ID = sereServBHYT.HEIN_SERVICE_TYPE_ID.Value;
                            heinServiceType.HEIN_SERVICE_TYPE_NAME = hEIN_SERVICE_TYPE_NAME;
                            heinServiceType.NUM_ORDER = sereServBHYT.HEIN_SERVICE_TYPE_NUM_ORDER;
                            num++;
                        }
                        else if (sereServBHYT.HEIN_SERVICE_TYPE_ID == 3 || sereServBHYT.HEIN_SERVICE_TYPE_ID == 4 || sereServBHYT.HEIN_SERVICE_TYPE_ID == 19 || sereServBHYT.HEIN_SERVICE_TYPE_ID == 20)
                        {
                            List<HeinServiceTypeADO> list5 = list.Where((HeinServiceTypeADO o) => o.KEY_PATY_ALTER == heinServiceType.KEY_PATY_ALTER && o.ID == 125 && o.GROUP_DEPARTMENT_ID == heinServiceType.GROUP_DEPARTMENT_ID).ToList();
                            if (list5 != null && list5.Count > 0)
                            {
                                continue;
                            }
                            heinServiceType.ID = 125L;
                            heinServiceType.HEIN_SERVICE_TYPE_NAME = "Giường";
                            heinServiceType.NUM_ORDER = (int)sereServBHYT.HEIN_SERVICE_TYPE_NUM_ORDER.Value;
                        }
                        else
                        {
                            heinServiceType.ID = sereServBHYT.HEIN_SERVICE_TYPE_ID.Value;
                            heinServiceType.HEIN_SERVICE_TYPE_NAME = sereServBHYT.HEIN_SERVICE_TYPE_NAME;
                            heinServiceType.NUM_ORDER = sereServBHYT.HEIN_SERVICE_TYPE_NUM_ORDER;
                        }
                    }
                    else
                    {
                        heinServiceType.HEIN_SERVICE_TYPE_NAME = "Khác";
                    }
                    list.Add(heinServiceType);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return list;
        }

        private List<HeinServiceTypeADO> HeinServiceTypeBedProcess(List<SereServADO> sereServAdos)
        {
            List<HeinServiceTypeADO> list = new List<HeinServiceTypeADO>();
            try
            {
                var list2 = (from o in sereServAdos
                             orderby o.HEIN_SERVICE_TYPE_NUM_ORDER ?? 99999999
                             group o by new { o.HEIN_SERVICE_TYPE_ID, o.KEY_PATY_ALTER, o.MEDICINE_LINE_ID, o.HEIN_SERVICE_TYPE_PARENT_1_ID, o.GROUP_DEPARTMENT_ID }).ToList();
                foreach (var item in list2)
                {
                    HeinServiceTypeADO heinServiceTypeADO = new HeinServiceTypeADO();
                    heinServiceTypeADO.KEY_PATY_ALTER = item.First().KEY_PATY_ALTER;
                    heinServiceTypeADO.PARENT_ID = item.First().HEIN_SERVICE_TYPE_ID;
                    heinServiceTypeADO.ID = item.First().HEIN_SERVICE_TYPE_PARENT_1_ID;
                    heinServiceTypeADO.MEDICINE_LINE_ID = item.First().MEDICINE_LINE_ID;
                    heinServiceTypeADO.GROUP_DEPARTMENT_ID = item.First().GROUP_DEPARTMENT_ID;
                    if (heinServiceTypeADO.PARENT_ID.HasValue && heinServiceTypeADO.PARENT_ID == 125)
                    {
                        heinServiceTypeADO.HEIN_SERVICE_TYPE_NAME = item.First().HEIN_SERVICE_TYPE_NAME;
                        heinServiceTypeADO.NUM_ORDER = item.First().HEIN_SERVICE_TYPE_NUM_ORDER;
                        heinServiceTypeADO.TOTAL_PRICE_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.VIR_TOTAL_PRICE_NO_EXPEND);
                        heinServiceTypeADO.TOTAL_PRICE_BHYT_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.TOTAL_PRICE_BHYT);
                        heinServiceTypeADO.TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.VIR_TOTAL_HEIN_PRICE.Value);
                        heinServiceTypeADO.TOTAL_PATIENT_PRICE_VIR_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE.Value);
                        heinServiceTypeADO.TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE_BHYT.Value);
                        heinServiceTypeADO.TOTAL_PATIENT_PRICE_SELF_HEIN_SERVICE_TYPE = item.Sum((SereServADO o) => o.TOTAL_PRICE_PATIENT_SELF);
                        heinServiceTypeADO.OTHER_SOURCE_PRICE = item.Sum((SereServADO o) => o.OTHER_SOURCE_PRICE);
                        heinServiceTypeADO.TOTAL_PATIENT_PRICE_LEFT = item.Sum((SereServADO o) => o.TOTAL_PATIENT_PRICE_LEFT);
                        heinServiceTypeADO.TOTAL_PRICE_VP = item.Sum((SereServADO o) => o.TOTAL_PRICE_VP);
                    }
                    list.Add(heinServiceTypeADO);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            return list;
        }

        private List<MedicineLineADO> MedicineLineProcesss(List<SereServADO> sereServAdos)
        {
            List<MedicineLineADO> list = new List<MedicineLineADO>();
            try
            {
                var list2 = (from o in sereServAdos
                             orderby o.MEDICINE_LINE_ID
                             group o by new { o.MEDICINE_LINE_ID, o.HEIN_SERVICE_TYPE_ID, o.KEY_PATY_ALTER, o.GROUP_DEPARTMENT_ID }).ToList();
                foreach (var item in list2)
                {
                    SereServADO sereServADO = item.FirstOrDefault();
                    MedicineLineADO medicineLineADO = new MedicineLineADO();
                    medicineLineADO.ID = sereServADO.MEDICINE_LINE_ID;
                    medicineLineADO.HEIN_SERVICE_TYPE_ID = sereServADO.HEIN_SERVICE_TYPE_ID;
                    medicineLineADO.KEY_PATY_ALTER = sereServADO.KEY_PATY_ALTER;
                    medicineLineADO.MEDICINE_LINE_CODE = sereServADO.MEDICINE_LINE_CODE;
                    medicineLineADO.MEDICINE_LINE_NAME = sereServADO.MEDICINE_LINE_NAME;
                    medicineLineADO.GROUP_DEPARTMENT_ID = sereServADO.GROUP_DEPARTMENT_ID;
                    if (sereServADO.MEDICINE_LINE_ID <= 0 && sereServADO.HEIN_SERVICE_TYPE_ID > 0)
                    {
                        medicineLineADO.MEDICINE_LINE_CODE = "Chưa xác định";
                        medicineLineADO.MEDICINE_LINE_NAME = "Chưa xác định";
                    }
                    if (rdo.ServiceReqs != null && rdo.ServiceReqs.Count > 0)
                    {
                        List<long> serviceReqIds = item.Select((SereServADO o) => o.SERVICE_REQ_ID.GetValueOrDefault()).ToList();
                        List<HIS_SERVICE_REQ> list3 = rdo.ServiceReqs.Where((HIS_SERVICE_REQ o) => serviceReqIds.Contains(o.ID) && o.REMEDY_COUNT.HasValue).ToList();
                        if (list3 != null && list3.Count > 0)
                        {
                            medicineLineADO.REMEDY_COUNT = list3.Sum((HIS_SERVICE_REQ o) => o.REMEDY_COUNT.GetValueOrDefault());
                        }
                    }
                    list.Add(medicineLineADO);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            return list;
        }

        private List<PatyAlterBhytADO> PatyAlterProcess(List<SereServADO> sereServAdos)
        {
            List<PatyAlterBhytADO> list = new List<PatyAlterBhytADO>();
            try
            {
                if (sereServAdos != null && sereServAdos.Count > 0)
                {
                    IEnumerable<IGrouping<string, SereServADO>> enumerable = from o in sereServAdos
                                                                             group o by o.KEY_PATY_ALTER;
                    foreach (IGrouping<string, SereServADO> item in enumerable)
                    {
                        PatyAlterBhytADO patyAlterBhytADO = new PatyAlterBhytADO();
                        patyAlterBhytADO = DataRawProcess.PatyAlterBHYTRawToADO(item.OrderBy((SereServADO o) => o.TDL_INTRUCTION_TIME).First().PatientTypeAlter, rdo.PatientTypeAlterAlls, rdo.Treatment, rdo.Branch, rdo.TreatmentTypes, rdo.CurrentPatyAlter, item.ToList());
                        patyAlterBhytADO.KEY = item.First().KEY_PATY_ALTER;
                        patyAlterBhytADO.TOTAL_PRICE = item.Sum((SereServADO o) => o.VIR_TOTAL_PRICE_NO_EXPEND);
                        patyAlterBhytADO.TOTAL_PRICE_BHYT = item.Sum((SereServADO o) => o.TOTAL_PRICE_BHYT);
                        patyAlterBhytADO.TOTAL_PRICE_HEIN = item.Sum((SereServADO o) => o.VIR_TOTAL_HEIN_PRICE.GetValueOrDefault());
                        patyAlterBhytADO.TOTAL_PRICE_PATIENT = item.Sum((SereServADO o) => o.VIR_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault());
                        patyAlterBhytADO.TOTAL_PRICE_PATIENT_SELF = item.Sum((SereServADO o) => o.TOTAL_PRICE_PATIENT_SELF);
                        patyAlterBhytADO.TOTAL_PRICE_OTHER = item.Sum((SereServADO o) => o.OTHER_SOURCE_PRICE);
                        patyAlterBhytADO.TOTAL_PATIENT_PRICE_LEFT = item.Sum((SereServADO o) => o.TOTAL_PATIENT_PRICE_LEFT);
                        patyAlterBhytADO.TOTAL_PRICE_VP = item.Sum((SereServADO o) => o.TOTAL_PRICE_VP);
                        patyAlterBhytADO.TOTAL_BHYT_PRICE = patyAlterBhytADO.TOTAL_PRICE_HEIN.GetValueOrDefault() + patyAlterBhytADO.TOTAL_PRICE_PATIENT.GetValueOrDefault();
                        if (item.First().PatientTypeAlter != null && item.First().PatientTypeAlter.LEVEL_CODE == "2" && item.First().PatientTypeAlter.RIGHT_ROUTE_CODE == "TT" && rdo.Treatment.TDL_TREATMENT_TYPE_ID == 3)
                        {
                            patyAlterBhytADO.RATIO_STR = (int)((item.FirstOrDefault((SereServADO o) => o.HEIN_RATIO.HasValue && !o.STENT_ORDER.HasValue) ?? item.First()).HEIN_RATIO.GetValueOrDefault() * 100m) + "%";
                        }
                        list.Add(patyAlterBhytADO);
                    }
                    if (list != null && list.Count > 0)
                    {
                        list = (from o in list
                                orderby o.LOG_TIME, o.KEY
                                select o).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            return list;
        }

        private void GroupDepartmentProcess()
        {
            try
            {
                GroupDepartments = new List<GroupDepartmentADO>();
                if (sereServADOs == null || sereServADOs.Count <= 0)
                {
                    return;
                }
                var list = (from o in sereServADOs
                            group o by new { o.KEY_PATY_ALTER, o.GROUP_DEPARTMENT_ID }).ToList();
                foreach (var g in list)
                {
                    GroupDepartmentADO groupDepartmentADO = new GroupDepartmentADO();
                    groupDepartmentADO.KEY_PATY_ALTER = g.First().KEY_PATY_ALTER;
                    groupDepartmentADO.TOTAL_PRICE_HEIN_SERVICE_TYPE = g.Sum((SereServADO o) => ((HIS_SERE_SERV)o).VIR_TOTAL_PRICE_NO_EXPEND);
                    groupDepartmentADO.TOTAL_PRICE_BHYT_HEIN_SERVICE_TYPE = g.Sum((SereServADO o) => o.TOTAL_PRICE_BHYT);       
                    groupDepartmentADO.TOTAL_PRICE_VP = g.Sum((SereServADO o) => o.TOTAL_PRICE_VP);
                    groupDepartmentADO.TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE = g.Sum((SereServADO o) => ((HIS_SERE_SERV)o).VIR_TOTAL_HEIN_PRICE.Value);
                    groupDepartmentADO.TOTAL_PATIENT_PRICE_VIR_HEIN_SERVICE_TYPE = g.Sum((SereServADO o) => ((HIS_SERE_SERV)o).VIR_TOTAL_PATIENT_PRICE.Value);
                    groupDepartmentADO.TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE = g.Sum((SereServADO o) => ((HIS_SERE_SERV)o).VIR_TOTAL_PATIENT_PRICE_BHYT.Value);
                    groupDepartmentADO.TOTAL_PATIENT_PRICE_SELF_HEIN_SERVICE_TYPE = g.Sum((SereServADO o) => o.TOTAL_PRICE_PATIENT_SELF);
                    groupDepartmentADO.OTHER_SOURCE_PRICE = g.Sum((SereServADO o) => ((HIS_SERE_SERV)o).OTHER_SOURCE_PRICE);
                    groupDepartmentADO.TOTAL_PATIENT_PRICE_LEFT = g.Sum((SereServADO o) => o.TOTAL_PATIENT_PRICE_LEFT);
                    groupDepartmentADO.GROUP_DEPARTMENT_ID = g.First().GROUP_DEPARTMENT_ID;
                    HIS_DEPARTMENT val = rdo.Departments.FirstOrDefault((HIS_DEPARTMENT o) => o.ID == g.First().GROUP_DEPARTMENT_ID);
                    if (val != null)
                    {
                        groupDepartmentADO.DEPARTMENT_CODE = val.DEPARTMENT_CODE;
                        groupDepartmentADO.DEPARTMENT_NAME = val.DEPARTMENT_NAME;
                    }
                    GroupDepartments.Add(groupDepartmentADO);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
