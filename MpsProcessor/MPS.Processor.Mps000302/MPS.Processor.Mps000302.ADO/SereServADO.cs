using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using MOS.EFMODEL.DataModels;
using MPS.Processor.Mps000302.PDO;
using MPS.Processor.Mps000302.PDO.Config;

namespace MPS.Processor.Mps000302.ADO
{
    public class SereServADO : HIS_SERE_SERV
    {
        public long SERVICE_TYPE_ID { get; set; }

        public string SERVICE_TYPE_CODE { get; set; }

        public string SERVICE_TYPE_NAME { get; set; }

        public string SERVICE_CODE { get; set; }

        public string SERVICE_NAME { get; set; }

        public string SERVICE_UNIT_CODE { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }

        public long? HEIN_SERVICE_TYPE_ID { get; set; }

        public string HEIN_SERVICE_TYPE_NAME { get; set; }

        public string HEIN_SERVICE_TYPE_CODE { get; set; }

        public long? HEIN_SERVICE_TYPE_NUM_ORDER { get; set; }

        public string EXECUTE_ROOM_NAME { get; set; }

        public string EXECUTE_ROOM_CODE { get; set; }

        public string HEIN_SERVICE_BHYT_CODE { get; set; }

        public string HEIN_SERVICE_BHYT_NAME { get; set; }

        public string ACTIVE_INGR_BHYT_CODE { get; set; }

        public string ACTIVE_INGR_BHYT_NAME { get; set; }

        public string CONCENTRA { get; set; }

        public long? NUMBER_OF_FILM { get; set; }

        public int ROW_POS { get; set; }

        public decimal PRICE_BHYT { get; set; }

        public decimal TOTAL_PRICE_BHYT { get; set; }

        public decimal TOTAL_PRICE_PATIENT_SELF { get; set; }

        public decimal RADIO_SERIVCE { get; set; }

        public decimal? TOTAL_PRICE_DEPARTMENT { get; set; }

        public decimal? TOTAL_PATIENT_PRICE_DEPARTMENT { get; set; }

        public decimal? TOTAL_HEIN_PRICE_DEPARTMENT { get; set; }

        public decimal? TOTAL_PRICE_ROOM { get; set; }

        public decimal? TOTAL_PATIENT_PRICE_ROOM { get; set; }

        public decimal? TOTAL_HEIN_PRICE_ROOM { get; set; }

        public decimal? TOTAL_HEIN_PRICE_ONE_AMOUNT { get; set; }

        public decimal? PRICE_CO_PAYMENT { get; set; }

        public long? MEDICINE_LINE_ID { get; set; }

        public string MEDICINE_LINE_CODE { get; set; }

        public string MEDICINE_LINE_NAME { get; set; }

        public HIS_PATIENT_TYPE_ALTER PatientTypeAlter { get; set; }

        public string KEY_PATY_ALTER { get; set; }

        public decimal? SERVICE_PAY_RATE { get; set; }

        public decimal? BHYT_PAY_RATE { get; set; }

        public long? HEIN_SERVICE_TYPE_PARENT_1_ID { get; set; }

        public decimal PRICE_VP { get; set; }

        public decimal TOTAL_PRICE_VP { get; set; }

        public decimal TOTAL_PATIENT_PRICE_LEFT { get; set; }

        public long? HEIN_SERVICE_TYPE_CHILD_NUM_ORDER { get; set; }

        public long IS_PAID { get; set; }

        public bool IsHide { get; set; }

        public long GROUP_DEPARTMENT_ID { get; set; }

        public SereServADO(HIS_SERE_SERV data, List<HIS_SERE_SERV> SereServs, List<HIS_SERE_SERV_EXT> sereServExts, List<HIS_HEIN_SERVICE_TYPE> heinServiceTypes, List<V_HIS_SERVICE> services, List<V_HIS_ROOM> rooms, List<HIS_MEDICINE_TYPE> medicineTypes, List<HIS_MEDICINE_LINE> medicineLines, List<HIS_MATERIAL_TYPE> materialTypes, PatientTypeCFG patientTypeCFG, HisConfigValue hisConfigValue, List<HIS_SERVICE_UNIT> hisServiceUnit, V_HIS_TREATMENT treatment, List<HIS_PATIENT_TYPE_ALTER> ListPta, List<HIS_PATIENT_TYPE> ListPatientType, bool groupSuatAn, List<HIS_SERE_SERV_BILL> sereServBills, List<HIS_SERE_SERV_DEPOSIT> sereServDeposits, List<HIS_SESE_DEPO_REPAY> seseDepoRepays)
        {
            SereServADO sereServADO = this;
            try
            {
                PropertyInfo[] array = Properties.Get<HIS_SERE_SERV>();
                PropertyInfo[] array2 = array;
                foreach (PropertyInfo propertyInfo in array2)
                {
                    propertyInfo.SetValue(this, propertyInfo.GetValue(data));
                }
                if (heinServiceTypes != null && heinServiceTypes.Count > 0 && services != null && services.Count > 0)
                {
                    V_HIS_SERVICE service = services.FirstOrDefault((V_HIS_SERVICE o) => o.ID == data.SERVICE_ID);
                    if (service != null)
                    {
                        HIS_HEIN_SERVICE_TYPE hIS_HEIN_SERVICE_TYPE = heinServiceTypes.FirstOrDefault((HIS_HEIN_SERVICE_TYPE o) => o.ID == service.HEIN_SERVICE_TYPE_ID);
                        SERVICE_TYPE_ID = service.SERVICE_TYPE_ID;
                        SERVICE_TYPE_CODE = service.SERVICE_TYPE_CODE;
                        SERVICE_TYPE_NAME = service.SERVICE_TYPE_NAME;
                        SERVICE_NAME = service.SERVICE_NAME;
                        SERVICE_CODE = service.SERVICE_CODE;
                        SERVICE_UNIT_CODE = service.SERVICE_UNIT_CODE;
                        SERVICE_UNIT_NAME = service.SERVICE_UNIT_NAME;
                        ACTIVE_INGR_BHYT_CODE = service.ACTIVE_INGR_BHYT_CODE;
                        ACTIVE_INGR_BHYT_NAME = service.ACTIVE_INGR_BHYT_NAME;
                        HEIN_SERVICE_BHYT_CODE = service.HEIN_SERVICE_BHYT_CODE;
                        HEIN_SERVICE_BHYT_NAME = service.HEIN_SERVICE_BHYT_NAME;
                        CONCENTRA = service.CONCENTRA;
                        HIS_SERE_SERV_EXT hIS_SERE_SERV_EXT = ((sereServExts != null) ? (from o in sereServExts
                                                                                         where o.SERE_SERV_ID == data.ID
                                                                                         orderby o.CREATE_TIME descending
                                                                                         select o).FirstOrDefault() : null);
                        if (hIS_SERE_SERV_EXT != null && hIS_SERE_SERV_EXT.NUMBER_OF_FILM.GetValueOrDefault() > 0)
                        {
                            NUMBER_OF_FILM = hIS_SERE_SERV_EXT.NUMBER_OF_FILM;
                        }
                        else
                        {
                            NUMBER_OF_FILM = service.NUMBER_OF_FILM;
                        }
                        if (hIS_HEIN_SERVICE_TYPE != null)
                        {
                            if (service.HEIN_SERVICE_TYPE_ID == 9 || service.HEIN_SERVICE_TYPE_ID == 10 || service.HEIN_SERVICE_TYPE_ID == 11)
                            {
                                HIS_HEIN_SERVICE_TYPE hIS_HEIN_SERVICE_TYPE2 = heinServiceTypes.FirstOrDefault((HIS_HEIN_SERVICE_TYPE o) => o.ID == 10);
                                HEIN_SERVICE_TYPE_ID = 123L;
                                HEIN_SERVICE_TYPE_NUM_ORDER = hIS_HEIN_SERVICE_TYPE2.NUM_ORDER;
                                HEIN_SERVICE_TYPE_NAME = "Thuốc, dịch truyền";
                            }
                            else if (SERVICE_TYPE_ID == 7 && !base.PARENT_ID.HasValue)
                            {
                                HIS_HEIN_SERVICE_TYPE hIS_HEIN_SERVICE_TYPE3 = heinServiceTypes.FirstOrDefault((HIS_HEIN_SERVICE_TYPE o) => o.ID == 16);
                                HEIN_SERVICE_TYPE_ID = 124L;
                                HEIN_SERVICE_TYPE_NUM_ORDER = hIS_HEIN_SERVICE_TYPE3.NUM_ORDER;
                                HEIN_SERVICE_TYPE_NAME = "Vật tư y tế";
                            }
                            else if (service.HEIN_SERVICE_TYPE_ID == 2 || service.HEIN_SERVICE_TYPE_ID == 7 || service.HEIN_SERVICE_TYPE_ID == 22)
                            {
                                HIS_HEIN_SERVICE_TYPE hIS_HEIN_SERVICE_TYPE4 = heinServiceTypes.FirstOrDefault((HIS_HEIN_SERVICE_TYPE o) => o.ID == 7);
                                HIS_HEIN_SERVICE_TYPE hIS_HEIN_SERVICE_TYPE5 = heinServiceTypes.FirstOrDefault((HIS_HEIN_SERVICE_TYPE o) => o.ID == 22);
                                HEIN_SERVICE_TYPE_ID = hIS_HEIN_SERVICE_TYPE4.ID;
                                HEIN_SERVICE_TYPE_NUM_ORDER = hIS_HEIN_SERVICE_TYPE4.VIR_PARENT_NUM_ORDER;
                                HEIN_SERVICE_TYPE_CHILD_NUM_ORDER = hIS_HEIN_SERVICE_TYPE4.NUM_ORDER;
                                HEIN_SERVICE_TYPE_CODE = hIS_HEIN_SERVICE_TYPE4.HEIN_SERVICE_TYPE_CODE;
                                HEIN_SERVICE_TYPE_NAME = hIS_HEIN_SERVICE_TYPE5.HEIN_SERVICE_TYPE_NAME.First().ToString().ToUpper() + hIS_HEIN_SERVICE_TYPE5.HEIN_SERVICE_TYPE_NAME.ToLower().Substring(1) + ", " + hIS_HEIN_SERVICE_TYPE4.HEIN_SERVICE_TYPE_NAME.ToLower();
                            }
                            else if (service.HEIN_SERVICE_TYPE_ID == 6 || service.HEIN_SERVICE_TYPE_ID == 21)
                            {
                                HIS_HEIN_SERVICE_TYPE hIS_HEIN_SERVICE_TYPE6 = heinServiceTypes.FirstOrDefault((HIS_HEIN_SERVICE_TYPE o) => o.ID == 6);
                                HIS_HEIN_SERVICE_TYPE hIS_HEIN_SERVICE_TYPE7 = heinServiceTypes.FirstOrDefault((HIS_HEIN_SERVICE_TYPE o) => o.ID == 21);
                                HEIN_SERVICE_TYPE_ID = hIS_HEIN_SERVICE_TYPE6.ID;
                                HEIN_SERVICE_TYPE_NUM_ORDER = hIS_HEIN_SERVICE_TYPE6.VIR_PARENT_NUM_ORDER;
                                HEIN_SERVICE_TYPE_CHILD_NUM_ORDER = hIS_HEIN_SERVICE_TYPE6.NUM_ORDER;
                                HEIN_SERVICE_TYPE_CODE = hIS_HEIN_SERVICE_TYPE6.HEIN_SERVICE_TYPE_CODE;
                                HEIN_SERVICE_TYPE_NAME = hIS_HEIN_SERVICE_TYPE6.HEIN_SERVICE_TYPE_NAME.First().ToString().ToUpper() + hIS_HEIN_SERVICE_TYPE6.HEIN_SERVICE_TYPE_NAME.ToLower().Substring(1) + ", " + hIS_HEIN_SERVICE_TYPE7.HEIN_SERVICE_TYPE_NAME.ToLower();
                            }
                            else
                            {
                                HEIN_SERVICE_TYPE_ID = hIS_HEIN_SERVICE_TYPE.ID;
                                HEIN_SERVICE_TYPE_NUM_ORDER = hIS_HEIN_SERVICE_TYPE.VIR_PARENT_NUM_ORDER;
                                HEIN_SERVICE_TYPE_CODE = hIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE;
                                HEIN_SERVICE_TYPE_NAME = hIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_NAME;
                            }
                        }
                        if (SERVICE_TYPE_ID == 7 && base.PARENT_ID.HasValue)
                        {
                            HIS_HEIN_SERVICE_TYPE hIS_HEIN_SERVICE_TYPE8 = heinServiceTypes.FirstOrDefault((HIS_HEIN_SERVICE_TYPE o) => o.ID == 14);
                            HEIN_SERVICE_TYPE_ID = base.PARENT_ID;
                            HEIN_SERVICE_TYPE_NUM_ORDER = hIS_HEIN_SERVICE_TYPE8.NUM_ORDER;
                            HEIN_SERVICE_TYPE_NAME = "Gói vật tư y tế";
                        }
                        if (medicineTypes != null && medicineTypes.Count > 0 && medicineLines != null && medicineLines.Count > 0)
                        {
                            HIS_MEDICINE_TYPE medicineType = medicineTypes.FirstOrDefault((HIS_MEDICINE_TYPE o) => o.SERVICE_ID == sereServADO.SERVICE_ID);
                            if (medicineType != null && medicineType.MEDICINE_LINE_ID.HasValue)
                            {
                                HIS_MEDICINE_LINE hIS_MEDICINE_LINE = medicineLines.FirstOrDefault((HIS_MEDICINE_LINE o) => o.ID == medicineType.MEDICINE_LINE_ID);
                                if (hIS_MEDICINE_LINE != null)
                                {
                                    MEDICINE_LINE_ID = hIS_MEDICINE_LINE.ID;
                                    MEDICINE_LINE_CODE = hIS_MEDICINE_LINE.MEDICINE_LINE_CODE;
                                    MEDICINE_LINE_NAME = hIS_MEDICINE_LINE.MEDICINE_LINE_NAME;
                                }
                            }
                        }
                        if (groupSuatAn && service.SERVICE_TYPE_ID == 16 && ListPatientType != null && ListPatientType.Count > 0)
                        {
                            HIS_PATIENT_TYPE hIS_PATIENT_TYPE = ListPatientType.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == sereServADO.PATIENT_TYPE_ID);
                            if (hIS_PATIENT_TYPE != null)
                            {
                                base.SERVICE_ID = -hIS_PATIENT_TYPE.ID;
                                HEIN_SERVICE_BHYT_CODE = hIS_PATIENT_TYPE.PATIENT_TYPE_CODE;
                                HEIN_SERVICE_BHYT_NAME = hIS_PATIENT_TYPE.PATIENT_TYPE_NAME;
                                SERVICE_CODE = hIS_PATIENT_TYPE.PATIENT_TYPE_CODE;
                                SERVICE_NAME = hIS_PATIENT_TYPE.PATIENT_TYPE_NAME;
                                base.TDL_SERVICE_CODE = hIS_PATIENT_TYPE.PATIENT_TYPE_CODE;
                                base.TDL_SERVICE_NAME = hIS_PATIENT_TYPE.PATIENT_TYPE_NAME;
                            }
                        }
                    }
                }
                if (rooms != null && rooms.Count > 0)
                {
                    V_HIS_ROOM v_HIS_ROOM = rooms.FirstOrDefault((V_HIS_ROOM o) => o.ID == data.TDL_EXECUTE_ROOM_ID);
                    if (v_HIS_ROOM != null)
                    {
                        EXECUTE_ROOM_CODE = v_HIS_ROOM.ROOM_CODE;
                        EXECUTE_ROOM_NAME = v_HIS_ROOM.ROOM_NAME;
                    }
                }
                string key = "";
                PatientTypeAlter = PatientTypeAlterProcessor.GetPatientTypeAlter(data, patientTypeCFG, treatment.TDL_TREATMENT_TYPE_ID.GetValueOrDefault(), ref key);
                KEY_PATY_ALTER = key;
                if (data.PATIENT_TYPE_ID != patientTypeCFG.PATIENT_TYPE__BHYT && PatientTypeAlter != null)
                {
                    KEY_PATY_ALTER = null;
                    PatientTypeAlter = null;
                }
                if (hisConfigValue != null && hisConfigValue.IsMergeServiceNotHein && string.IsNullOrWhiteSpace(KEY_PATY_ALTER) && ListPta.Count > 0)
                {
                    HIS_PATIENT_TYPE_ALTER hIS_PATIENT_TYPE_ALTER = ListPta.FirstOrDefault((HIS_PATIENT_TYPE_ALTER o) => o.LOG_TIME <= sereServADO.TDL_INTRUCTION_TIME);
                    if (hIS_PATIENT_TYPE_ALTER == null)
                    {
                        hIS_PATIENT_TYPE_ALTER = ListPta.First();
                    }
                    KEY_PATY_ALTER = PatientTypeAlterProcessor.ToString(hIS_PATIENT_TYPE_ALTER, data, treatment.TDL_TREATMENT_TYPE_ID.GetValueOrDefault());
                    PatientTypeAlter = hIS_PATIENT_TYPE_ALTER;
                }
                if (base.VIR_TOTAL_HEIN_PRICE.HasValue)
                {
                    TOTAL_HEIN_PRICE_ONE_AMOUNT = base.VIR_TOTAL_HEIN_PRICE.Value / base.AMOUNT;
                }
                RADIO_SERIVCE = ((!(base.ORIGINAL_PRICE > 0m)) ? 0m : (base.HEIN_LIMIT_PRICE.HasValue ? (base.HEIN_LIMIT_PRICE.Value / base.ORIGINAL_PRICE * 100m) : (base.PRICE / base.ORIGINAL_PRICE * 100m)));
                decimal? num = null;
                num = (base.HEIN_LIMIT_PRICE.HasValue ? new decimal?(100m * Math.Round(base.HEIN_LIMIT_PRICE.Value / (base.ORIGINAL_PRICE * (1m + base.VAT_RATIO)), 2)) : ((!base.LIMIT_PRICE.HasValue) ? new decimal?(100m * Math.Round(base.PRICE / base.ORIGINAL_PRICE, 2)) : new decimal?(100m * Math.Round(base.LIMIT_PRICE.Value / (base.ORIGINAL_PRICE * (1m + base.VAT_RATIO)), 2))));
                if (SERVICE_TYPE_ID == 6 || SERVICE_TYPE_ID == 7)
                {
                    SERVICE_PAY_RATE = 100;
                    BHYT_PAY_RATE = num;
                }
                else
                {
                    SERVICE_PAY_RATE = num;
                    BHYT_PAY_RATE = 100;
                }
                PRICE_BHYT = PriceBHYTProcess(this, materialTypes);
                TOTAL_PRICE_BHYT = PRICE_BHYT * base.AMOUNT * (BHYT_PAY_RATE.GetValueOrDefault() / 100m) * (SERVICE_PAY_RATE.GetValueOrDefault() / 100m);
                if (!base.PRIMARY_PRICE.HasValue)
                {
                    base.PRIMARY_PRICE = base.VIR_PRICE;
                }
                else
                {
                    base.PRIMARY_PRICE = Math.Round(base.PRIMARY_PRICE.GetValueOrDefault() * (1m + base.VAT_RATIO), 4, MidpointRounding.AwayFromZero);
                }
                decimal pRICE_BHYT;
                decimal? pRIMARY_PRICE;
                if (hisConfigValue != null && !hisConfigValue.IsPriceWithDifference)
                {
                    pRIMARY_PRICE = base.PRIMARY_PRICE;
                    pRICE_BHYT = PRICE_BHYT;
                    if (((pRIMARY_PRICE.GetValueOrDefault() > pRICE_BHYT) & pRIMARY_PRICE.HasValue) && SERVICE_TYPE_ID == 1)
                    {
                        base.PRIMARY_PRICE = PRICE_BHYT;
                    }
                }
                base.OTHER_SOURCE_PRICE = base.OTHER_SOURCE_PRICE.GetValueOrDefault() * base.AMOUNT;
                if (base.IS_EXPEND == 1 && !groupSuatAn)
                {
                    base.PRIMARY_PRICE = default(decimal);
                }
                base.VIR_TOTAL_PRICE_NO_EXPEND = base.PRIMARY_PRICE * (decimal?)base.AMOUNT;
                TOTAL_PRICE_PATIENT_SELF = base.VIR_TOTAL_PRICE_NO_EXPEND.GetValueOrDefault() * (SERVICE_PAY_RATE.GetValueOrDefault() / 100m) - base.VIR_TOTAL_HEIN_PRICE.GetValueOrDefault() - base.VIR_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault() - base.OTHER_SOURCE_PRICE.GetValueOrDefault();
                if (TOTAL_PRICE_PATIENT_SELF < 0m)
                {
                    TOTAL_PRICE_PATIENT_SELF = 0m;
                }
                PRICE_VP = base.VIR_PRICE.GetValueOrDefault();
                TOTAL_PRICE_VP = base.VIR_TOTAL_PRICE.GetValueOrDefault();
                GROUP_DEPARTMENT_ID = ((HIS_SERE_SERV)this).TDL_REQUEST_DEPARTMENT_ID;
                TOTAL_PATIENT_PRICE_LEFT = base.VIR_TOTAL_PATIENT_PRICE.GetValueOrDefault() - base.VIR_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault() - base.OTHER_SOURCE_PRICE.GetValueOrDefault();
                pRIMARY_PRICE = SERVICE_PAY_RATE;
                pRICE_BHYT = 100;
                if ((pRIMARY_PRICE.GetValueOrDefault() < pRICE_BHYT) & pRIMARY_PRICE.HasValue)
                {
                    pRIMARY_PRICE = SERVICE_PAY_RATE;
                    if ((pRIMARY_PRICE.GetValueOrDefault() > default(decimal)) & pRIMARY_PRICE.HasValue)
                    {
                        if (HEIN_SERVICE_TYPE_ID == 5 || base.TDL_SERVICE_TYPE_ID == 1)
                        {
                            PRICE_VP /= SERVICE_PAY_RATE.GetValueOrDefault() / 100m;
                        }
                        else if (HeinServiceTypeExt.HEIN_BED__IDs.Contains(HEIN_SERVICE_TYPE_ID.GetValueOrDefault()) && base.SHARE_COUNT.HasValue && !base.HEIN_LIMIT_PRICE.HasValue)
                        {
                            PRICE_VP /= SERVICE_PAY_RATE.GetValueOrDefault() / 100m;
                        }
                    }
                }
                if (TOTAL_PATIENT_PRICE_LEFT < 0m)
                {
                    TOTAL_PATIENT_PRICE_LEFT = 0m;
                }
                HIS_SERVICE_UNIT svUnit = hisServiceUnit.FirstOrDefault((HIS_SERVICE_UNIT o) => o.ID == sereServADO.TDL_SERVICE_UNIT_ID);
                if (svUnit != null && svUnit.CONVERT_RATIO.HasValue && base.USE_ORIGINAL_UNIT_FOR_PRES != 1 && svUnit.CONVERT_RATIO.Value != 0m)
                {
                    HIS_SERVICE_UNIT hIS_SERVICE_UNIT = hisServiceUnit.FirstOrDefault((HIS_SERVICE_UNIT o) => o.ID == svUnit.CONVERT_ID);
                    if (hIS_SERVICE_UNIT != null)
                    {
                        SERVICE_UNIT_CODE = hIS_SERVICE_UNIT.SERVICE_UNIT_CODE;
                        SERVICE_UNIT_NAME = hIS_SERVICE_UNIT.SERVICE_UNIT_NAME;
                    }
                    base.AMOUNT *= svUnit.CONVERT_RATIO.Value;
                    base.PRICE /= svUnit.CONVERT_RATIO.Value;
                    base.PRIMARY_PRICE = base.PRIMARY_PRICE.GetValueOrDefault() / svUnit.CONVERT_RATIO.Value;
                    PRICE_BHYT /= svUnit.CONVERT_RATIO.Value;
                    PRICE_VP /= svUnit.CONVERT_RATIO.Value;
                }
                if (sereServBills != null && sereServBills.Count > 0 && sereServBills.Exists((HIS_SERE_SERV_BILL s) => s.SERE_SERV_ID == data.ID && s.IS_CANCEL != 1))
                {
                    IS_PAID = 1L;
                }
                else
                {
                    if (sereServDeposits == null || sereServDeposits.Count <= 0)
                    {
                        return;
                    }
                    List<HIS_SERE_SERV_DEPOSIT> list = sereServDeposits.Where((HIS_SERE_SERV_DEPOSIT o) => o.SERE_SERV_ID == data.ID).ToList();
                    if (list == null || list.Count <= 0)
                    {
                        return;
                    }
                    if (seseDepoRepays != null && seseDepoRepays.Count > 0)
                    {
                        list = list.Where((HIS_SERE_SERV_DEPOSIT o) => seseDepoRepays.Exists((HIS_SESE_DEPO_REPAY e) => e.SERE_SERV_DEPOSIT_ID == o.ID && e.IS_CANCEL == 1) || !seseDepoRepays.Exists((HIS_SESE_DEPO_REPAY e) => e.SERE_SERV_DEPOSIT_ID == o.ID)).ToList();
                    }
                    if (list != null && list.Count > 0)
                    {
                        IS_PAID = 1L;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private decimal? GetBHYTPayRate(SereServADO s)
        {
            decimal? num = null;
            try
            {
                if (s.HEIN_LIMIT_PRICE.HasValue)
                {
                    decimal oRIGINAL_PRICE = s.ORIGINAL_PRICE;
                    decimal? hEIN_LIMIT_PRICE = s.HEIN_LIMIT_PRICE;
                    if (!((oRIGINAL_PRICE > hEIN_LIMIT_PRICE.GetValueOrDefault()) & hEIN_LIMIT_PRICE.HasValue))
                    {
                        return Math.Round(s.ORIGINAL_PRICE / s.HEIN_LIMIT_PRICE.Value * 100m, 0);
                    }
                }
                return 100;
            }
            catch (Exception ex)
            {
                num = null;
                LogSystem.Warn(ex);
            }
            return num;
        }

        public decimal PriceBHYTProcess(SereServADO ss, List<HIS_MATERIAL_TYPE> materialTypes)
        {
            decimal result = default(decimal);
            try
            {
                if (!string.IsNullOrEmpty(ss.HEIN_CARD_NUMBER) && ss.PatientTypeAlter != null)
                {
                    decimal? vIR_TOTAL_HEIN_PRICE = ss.VIR_TOTAL_HEIN_PRICE;
                    if ((vIR_TOTAL_HEIN_PRICE.GetValueOrDefault() > default(decimal)) & vIR_TOTAL_HEIN_PRICE.HasValue)
                    {
                        return Math.Round(ss.ORIGINAL_PRICE * (1m + ss.VAT_RATIO), 4, MidpointRounding.AwayFromZero);
                    }
                    result = default(decimal);
                }
            }
            catch (Exception ex)
            {
                result = default(decimal);
                LogSystem.Warn(ex);
            }
            return result;
        }
    }
}
