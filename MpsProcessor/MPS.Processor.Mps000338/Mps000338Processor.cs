using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.ProcessorBase.Core;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MPS.Processor.Mps000338.PDO;
using FlexCel.Report;
using MPS.ProcessorBase;
using System.IO;
using MOS.Filter;
using Inventec.Common.Adapter;

namespace MPS.Processor.Mps000338
{
    public partial class Mps000338Processor : AbstractProcessor
    {
        Mps000338PDO rdo;
        public Mps000338Processor(CommonParam param, PrintData printData)
            : base(param, printData)
        {
            rdo = (Mps000338PDO)rdoBase;
        }

        private void SetBarcodeKey()
        {
            try
            {
                Inventec.Common.BarcodeLib.Barcode barcodeTreatment = new Inventec.Common.BarcodeLib.Barcode(rdo.Treatment.TREATMENT_CODE);
                barcodeTreatment.Alignment = Inventec.Common.BarcodeLib.AlignmentPositions.CENTER;
                barcodeTreatment.IncludeLabel = false;
                barcodeTreatment.Width = 120;
                barcodeTreatment.Height = 40;
                barcodeTreatment.RotateFlipType = RotateFlipType.Rotate180FlipXY;
                barcodeTreatment.LabelPosition = Inventec.Common.BarcodeLib.LabelPositions.BOTTOMCENTER;
                barcodeTreatment.EncodedType = Inventec.Common.BarcodeLib.TYPE.CODE128;
                barcodeTreatment.IncludeLabel = true;
                dicImage.Add(Mps000338ExtendSingleKey.TREATMENT_CODE_BAR, barcodeTreatment);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetSingleImage(string key, string imageUrl)
        {
            try
            {
                MemoryStream stream = Inventec.Fss.Client.FileDownload.GetFile(imageUrl);
                if (stream != null)
                {
                    SetSingleKey(new KeyValue(key, stream.ToArray()));
                }
                else
                {
                    SetSingleKey(new KeyValue(key, ""));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

        }

        private List<V_HIS_EKIP_USER> GetEkipUsers()
        {
            List<V_HIS_EKIP_USER> result = new List<V_HIS_EKIP_USER>();
            try
            {
                if (rdo.SereServ.EKIP_ID.HasValue && rdo.SereServ.EKIP_ID.Value > 0)
                {
                    CommonParam param = new CommonParam();
                    HisEkipFilter filter = new HisEkipFilter();
                    filter.ID = rdo.SereServ.EKIP_ID;
                    var lstEkip = new BackendAdapter(param).Get<List<HIS_EKIP>>("api/HisEkip/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, param);
                    if (lstEkip != null && lstEkip.Count > 0)
                    {
                        HisEkipUserViewFilter filterUser = new HisEkipUserViewFilter();
                        filterUser.EKIP_IDs = lstEkip.Select(o => o.ID).Distinct().ToList();
                        result = new BackendAdapter(param).Get<List<V_HIS_EKIP_USER>>("api/HisEkipUser/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filterUser, param);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return result;
        }

        public override bool ProcessData()
        {
            bool result = false;
            try
            {
                Inventec.Common.FlexCellExport.ProcessSingleTag singleTag = new Inventec.Common.FlexCellExport.ProcessSingleTag();
                Inventec.Common.FlexCellExport.ProcessBarCodeTag barCodeTag = new Inventec.Common.FlexCellExport.ProcessBarCodeTag();
                Inventec.Common.FlexCellExport.ProcessObjectTag objectTag = new Inventec.Common.FlexCellExport.ProcessObjectTag();

                store.ReadTemplate(System.IO.Path.GetFullPath(fileName));
                DataInputProcess();
                ProcessSingleKey();
                SetBarcodeKey();
                //SetNumOrderKey(GetNumOrderPrint(ProcessUniqueCodeData()));
                singleTag.ProcessData(store, singleValueDictionary);
                barCodeTag.ProcessData(store, dicImage);
                // tất cả
                objectTag.AddObjectData(store, "GroupAll", GroupAll);
                objectTag.AddObjectData(store, "SereServAll", this.ListSereServAll);
                objectTag.AddRelationship(store, "GroupAll", "SereServAll", "TDL_SERVICE_TYPE_ID", "TDL_SERVICE_TYPE_ID");
                // hao phí
                objectTag.AddObjectData(store, "GroupIsExpends", GroupIsExpends);
                objectTag.AddObjectData(store, "SereServIsExpend", this.ListSereServIsExpend);
                objectTag.AddRelationship(store, "GroupIsExpends", "SereServIsExpend", "TDL_SERVICE_TYPE_ID", "TDL_SERVICE_TYPE_ID");
                var lstEkip = GetEkipUsers();
                // đối tượng
                objectTag.AddObjectData(store, "lstEkip", lstEkip);
                objectTag.AddObjectData(store, "PatientTypes", this.PatientTypes);
                objectTag.AddObjectData(store, "GroupPatientTypes", this.GroupPatientTypes);
                objectTag.AddObjectData(store, "SereServPatientType", this.ListSereServPatientType);
                objectTag.AddRelationship(store, "PatientTypes", "GroupPatientTypes", "ID", "PATIENT_TYPE_ID");
                objectTag.AddRelationship(store, "GroupPatientTypes", "SereServPatientType", "TDL_SERVICE_TYPE_ID", "TDL_SERVICE_TYPE_ID");
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

            return result;
        }

        class ReplaceValueFunction : FlexCel.Report.TFlexCelUserFunction
        {
            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length <= 0)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                try
                {
                    string value = parameters[0] + "";
                    if (!String.IsNullOrEmpty(value))
                    {
                        value = value.Replace(';', '/');
                    }
                    return value;
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                    return parameters[0];
                }
            }
        }

        private void ProcessSingleKey()
        {
            try
            {
                AddObjectKeyIntoListkey<V_HIS_TREATMENT_FEE>(rdo.Treatment, true);
                AddObjectKeyIntoListkey<V_HIS_SERE_SERV>(rdo.SereServ, false);
                AddObjectKeyIntoListkey<HIS_SERVICE_REQ>(rdo.ServiceReq, false);
                AddObjectKeyIntoListkey<V_HIS_BED_LOG>(rdo.HisBedLog, false);
                AddObjectKeyIntoListkey<V_HIS_PATIENT_TYPE_ALTER>(rdo.CurrentPatyAlter, false);
                SetSingleKey(new KeyValue(Mps000338ExtendSingleKey.TOTAL_PRICE_IS_EXPEND, this.totalPriceIsExpend));
                SetSingleKey(new KeyValue(Mps000338ExtendSingleKey.TOTAL_PRICE_PATIENT_TYPE, this.totalPricePatientType));
                SetSingleKey(new KeyValue(Mps000338ExtendSingleKey.TOTAL_PRICE, totalPrice));
                SetSingleKey(new KeyValue(Mps000338ExtendSingleKey.AGE_STRING, Inventec.Common.DateTime.Calculation.AgeString(rdo.Treatment.TDL_PATIENT_DOB, "", "", "", "", rdo.Treatment.IN_TIME)));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
