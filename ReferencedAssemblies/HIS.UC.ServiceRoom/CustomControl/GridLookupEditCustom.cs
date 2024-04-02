using Inventec.Desktop.CustomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ServiceRoom.CustomControl
{
    public class GridLookupEditCustom : DevExpress.XtraEditors.GridLookUpEdit
    {
        public GridLookupEditCustom() : base() { }

        private bool autoComplate = false;
        //protected override bool IsAutoComplete
        //{
        //    get
        //    {
        //        return autoComplate;
        //    }
        //}

        internal void CustomSetAuto(bool auto)
        {
            autoComplate = auto;
        }
    }
    //public class GridLookupEditCustom : CustomGridLookUpEditWithFilterMultiColumn
    //{
    //    public GridLookupEditCustom() : base() { }

    //    private bool autoComplate = false;
    //    protected override bool IsAutoComplete
    //    {
    //        get
    //        {
    //            return autoComplate;
    //        }
    //    }

    //    internal void CustomSetAuto(bool auto)
    //    {
    //        autoComplate = auto;
    //    }
    //}
}
