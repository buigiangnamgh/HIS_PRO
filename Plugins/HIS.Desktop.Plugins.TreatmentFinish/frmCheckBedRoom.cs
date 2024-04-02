using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.Desktop.Plugins.TreatmentFinish
{
    public partial class frmCheckBedRoom : Form
    {
        long treatmentId;
        Inventec.Desktop.Common.Modules.Module currentModule;
        List<V_HIS_TREATMENT_BED_ROOM> listTreatmentBedRoom;

        public frmCheckBedRoom(Inventec.Desktop.Common.Modules.Module module, List<V_HIS_TREATMENT_BED_ROOM> treatmentBedRoom)
        {
            InitializeComponent();

            try
            {
                this.listTreatmentBedRoom = treatmentBedRoom;
                this.currentModule = module;
                this.Icon = Icon.ExtractAssociatedIcon(System.IO.Path.Combine(LocalStorage.Location.ApplicationStoreLocation.ApplicationDirectory, System.Configuration.ConfigurationManager.AppSettings["Inventec.Desktop.Icon"]));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void frmCheckBedRoom_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataToGrid();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void LoadDataToGrid()
        {
            try
            {
                gridControlCheckBedRoom.BeginUpdate();
                gridControlCheckBedRoom.DataSource = this.listTreatmentBedRoom;
                gridControlCheckBedRoom.EndUpdate();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridViewCheckBedRoom_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var row = (V_HIS_TREATMENT_BED_ROOM)gridViewCheckBedRoom.GetFocusedRow();
                if (row != null)
                {
                    List<object> listArgs = new List<object>();
                    listArgs.Add(row);

                    if (this.currentModule == null)
                    {
                        CallModule.Run(CallModule.BedHistory, 0, 0, listArgs);
                    }
                    else
                    {
                        CallModule.Run(CallModule.BedHistory, this.currentModule.RoomId, this.currentModule.RoomTypeId, listArgs);
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridViewCheckBedRoom_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                {
                    MOS.EFMODEL.DataModels.V_HIS_TREATMENT_BED_ROOM pData = (MOS.EFMODEL.DataModels.V_HIS_TREATMENT_BED_ROOM)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];

                    if (pData != null)
                    {
                        if (e.Column.FieldName == "STT")
                        {
                            e.Value = e.ListSourceRowIndex + 1; //+ ((pagingGrid.CurrentPage - 1) * pagingGrid.PageSize);
                        }
                        else if (e.Column.FieldName == "ADD_NAME")
                        {
                            e.Value = !string.IsNullOrEmpty(pData.ADD_LOGINNAME) ? pData.ADD_LOGINNAME + " - " + pData.ADD_USERNAME : "";
                        }
                        else if (e.Column.FieldName == "ADD_TIME_STR")
                        {
                            e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(pData.ADD_TIME);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
    }
}
