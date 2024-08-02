
using HIS.UC.ExamServiceAdd.Run;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MOS.SDO;
using HIS.UC.ExamServiceAdd.GetValue;
using MOS.EFMODEL.DataModels;
using HIS.UC.ExamServiceAdd.ADO;
using HIS.UC.ExamServiceAdd.GetValueV2;

namespace HIS.UC.ExamServiceAdd
{
    public class ExamServiceAddProcessor : BussinessBase
    {
        object uc;
        public ExamServiceAddProcessor()
            : base()
        {
        }

        public ExamServiceAddProcessor(CommonParam paramBusiness)
            : base(paramBusiness)
        {
        }

        public object Run(ExamServiceAddInitADO arg)
        {
            uc = null;
            try
            {
                IRun behavior = RunFactory.MakeIExamServiceAdd(param, arg);
                uc = behavior != null ? (behavior.Run()) : null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                uc = null;
            }
            return uc;
        }

        public object GetValue(UserControl control)
        {
            object result = null;
            try
            {
                IGetValue behavior = GetValueFactory.MakeIGetValue(param, (control == null ? (UserControl)uc : control));
                result = (behavior != null) ? behavior.Run() : null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        public object GetValueV2(UserControl control)
        {
            object result = null;
            try
            {
                IGetValueV2 behavior = GetValueV2Factory.MakeIGetValue(param, (control == null ? (UserControl)uc : control));
                result = (behavior != null) ? behavior.Run() : null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
