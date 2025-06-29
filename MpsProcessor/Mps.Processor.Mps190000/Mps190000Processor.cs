/* IVT
 * @Project : hisnguonmo
 * Copyright (C) 2017 INVENTEC
 *  
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 *  
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */
using HIS.Desktop.Common.BankQrCode;
using Inventec.Common.QRCoder;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MPS.Processor.Mps100494.PDO;
using MPS.Processor.Mps190000.PDO;
using MPS.ProcessorBase.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MPS.Processor.Mps190000
{
    public class Mps190000Processor : AbstractProcessor
    {
        Mps190000PDO rdo;

        public Mps190000Processor(CommonParam param, PrintData printData)
            : base(param, printData)
        {
            rdo = (Mps190000PDO)rdoBase;
        }

        private void SetQrCode()
        {
            try
            {
                if (rdo.borrowADO != null)
                {
                    string data = String.Format("Họ tên: {0} - Mã BN: {1} - Mã ĐT: {2} - khoa ĐT: {3} - Trạng thái HS: {4} - Số thẻ: {5} - TG mượn: {6}", rdo.borrowADO.treatment.TDL_PATIENT_NAME, rdo.borrowADO.treatment.TDL_PATIENT_CODE, rdo.borrowADO.treatment.TREATMENT_CODE, rdo.borrowADO.treatment.HOPITALIZE_DEPARTMENT_NAME, rdo.borrowADO.TREATMENT_STT_NAME, rdo.borrowADO.CARER_CARD_NUMBER, Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(rdo.borrowADO.TIME_FROM));
                    if (!String.IsNullOrWhiteSpace(data))
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                        BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
                        byte[] qrCodeImage = qrCode.GetGraphic(20);
                        SetSingleKey(new KeyValue(SingleKeyValue.BORROW_CODE_BAR, qrCodeImage));
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public override bool ProcessData()
        {
            bool result = false;
            try
            {
                Inventec.Common.FlexCellExport.ProcessSingleTag singleTag = new Inventec.Common.FlexCellExport.ProcessSingleTag();
                Inventec.Common.FlexCellExport.ProcessObjectTag objectTag = new Inventec.Common.FlexCellExport.ProcessObjectTag();

                store.ReadTemplate(System.IO.Path.GetFullPath(fileName));

                this.SetQrCode();

                if (rdo.borrowADO.treatment != null)
                {
                    AddObjectKeyIntoListkey<V_HIS_TREATMENT_4>(rdo.borrowADO.treatment, false);
                }
                singleTag.ProcessData(store, singleValueDictionary);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

            return result;
        }


    }
}
