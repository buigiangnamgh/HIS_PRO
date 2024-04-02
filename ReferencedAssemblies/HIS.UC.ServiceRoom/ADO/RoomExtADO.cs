using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ServiceRoom.ADO
{
    public class RoomExtADO // : V_HIS_EXECUTE_ROOM_1
    {
        public string EXECUTE_ROOM_CODE { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string EXECUTE_ROOM_NAME__UNSIGN { get; set; }
        public long ROOM_ID { get; set; }
        public long? NUM_ORDER { get; set; }
        public decimal? TOTAL_NEW_SERVICE_REQ { get; set; }
        public decimal? TOTAL_TODAY_SERVICE_REQ { get; set; }
        public decimal? TOTAL_WAIT_TODAY_SERVICE_REQ { get; set; }
        public decimal? TOTAL_OPEN_SERVICE_REQ { get; set; }
        public decimal? MAX_REQ_BHYT_BY_DAY { get; set; }
        public decimal? MAX_REQUEST_BY_DAY { get; set; }
        public decimal? TOTAL_END_SERVICE_REQ { get; set; }
        public string WORKING_LOGINNAME { get; set; }
        public string WORKING_USERNAME { get; set; }
        public string RESPONSIBLE_USERNAME { get; set; }
        public string RESPONSIBLE_LOGINNAME { get; set; }
        public string AMOUNT_COMBO { get; set; }
        public long IS_WARN { get; set; }
        public bool IsChecked { get; set; }
        public long? RESPONSIBLE_TIME { get; set; }

        public string NumOrderBlock { get; set; }
        public short? IsBlockNumOrder { get; set; }

        public RoomExtADO() : base() { }
        public RoomExtADO(L_HIS_ROOM_COUNTER data, List<V_HIS_ROOM> hisRooms, List<RoomExtADO> lRooms = null)
            : base()
        {
            this.EXECUTE_ROOM_CODE = data.EXECUTE_ROOM_CODE;
            this.EXECUTE_ROOM_NAME = data.EXECUTE_ROOM_NAME;
            this.EXECUTE_ROOM_NAME__UNSIGN = Inventec.Common.String.Convert.UnSignVNese(this.EXECUTE_ROOM_NAME);
            this.NUM_ORDER = data.NUM_ORDER;
            this.ROOM_ID = data.ROOM_ID;
            this.TOTAL_TODAY_SERVICE_REQ = (long?)data.TOTAL_TODAY_SERVICE_REQ;
            this.TOTAL_NEW_SERVICE_REQ = (long?)data.TOTAL_NEW_SERVICE_REQ;
            this.TOTAL_WAIT_TODAY_SERVICE_REQ = (long?)data.TOTAL_WAIT_TODAY_SERVICE_REQ;
            this.MAX_REQ_BHYT_BY_DAY = (long?)data.MAX_REQ_BHYT_BY_DAY;
            this.MAX_REQUEST_BY_DAY = (long?)data.MAX_REQUEST_BY_DAY;
            this.TOTAL_OPEN_SERVICE_REQ = (long?)(data.TOTAL_TODAY_SERVICE_REQ - data.TOTAL_NEW_SERVICE_REQ);
            this.RESPONSIBLE_LOGINNAME = data.RESPONSIBLE_LOGINNAME;
            this.RESPONSIBLE_USERNAME = data.RESPONSIBLE_USERNAME;
            this.TOTAL_END_SERVICE_REQ = (long?)data.TOTAL_END_SERVICE_REQ;
            long TOTAL_TODAY_1 = Convert.ToInt64(data.TOTAL_TODAY_SERVICE_REQ ?? 0);
            long MAX_BY_DAY_1 = data.MAX_REQUEST_BY_DAY ?? 0;
            long MAX_BY_DAY_2 = data.MAX_REQ_BHYT_BY_DAY ?? 0;

            if ((MAX_BY_DAY_1 > 0 && MAX_BY_DAY_1 < TOTAL_TODAY_1) || (MAX_BY_DAY_2 > 0 && MAX_BY_DAY_2 < TOTAL_TODAY_1))
            {
                this.IS_WARN = 1;
            }

            this.RESPONSIBLE_TIME = data.RESPONSIBLE_TIME;
            this.WORKING_LOGINNAME = data.WORKING_LOGINNAME;
            this.WORKING_USERNAME = data.WORKING_USERNAME;
            this.IsBlockNumOrder = data.IS_BLOCK_NUM_ORDER;

            if (lRooms != null && lRooms.Count > 0)
            {
                var oldRoom = lRooms.FirstOrDefault(k => k.ROOM_ID == data.ROOM_ID && k.IsChecked);
                if (oldRoom != null)
                {
                    this.IsChecked = true;
                    this.NumOrderBlock = oldRoom.NumOrderBlock;
                }
            }
        }

        public RoomExtADO(V_HIS_EXECUTE_ROOM_1 data, List<V_HIS_ROOM> hisRooms)
            : base()
        {
            this.EXECUTE_ROOM_CODE = data.EXECUTE_ROOM_CODE;
            this.EXECUTE_ROOM_NAME = data.EXECUTE_ROOM_NAME;
            this.EXECUTE_ROOM_NAME__UNSIGN = Inventec.Common.String.Convert.UnSignVNese(this.EXECUTE_ROOM_NAME);
            this.NUM_ORDER = data.NUM_ORDER;
            this.ROOM_ID = data.ROOM_ID;

            long TOTAL_OPEN_1 = Convert.ToInt64(data.TOTAL_OPEN_SERVICE_REQ ?? 0);
            long TOTAL_TODAY_1 = Convert.ToInt64(data.TOTAL_TODAY_SERVICE_REQ ?? 0);
            long MAX_BY_DAY_1 = data.MAX_REQUEST_BY_DAY ?? 0;
            this.AMOUNT_COMBO = TOTAL_OPEN_1 + "/" + TOTAL_TODAY_1 + "(" + MAX_BY_DAY_1 + ")";
            long MAX_BY_DAY_2 = data.MAX_REQ_BHYT_BY_DAY ?? 0;
            this.RESPONSIBLE_LOGINNAME = data.RESPONSIBLE_LOGINNAME;
            this.RESPONSIBLE_USERNAME = data.RESPONSIBLE_USERNAME;
            this.IsBlockNumOrder = data.IS_BLOCK_NUM_ORDER;

            if ((MAX_BY_DAY_1 > 0 && MAX_BY_DAY_1 < TOTAL_TODAY_1) || (MAX_BY_DAY_2 > 0 && MAX_BY_DAY_2 < TOTAL_TODAY_1))
            {
                this.IS_WARN = 1;
            }

            //this.RESPONSIBLE_TIME = data.RESPONSIBLE_TIME;//TODO
            //this.WORKING_USERNAME = data.WORKING_USERNAME;//TODO

            var room = hisRooms != null && hisRooms.Count > 0 ? hisRooms.FirstOrDefault(o => o.ID == ROOM_ID) : null;
            if (room != null)
            {
                this.RESPONSIBLE_TIME = room.RESPONSIBLE_TIME;
                this.WORKING_LOGINNAME = room.WORKING_LOGINNAME;
                this.WORKING_USERNAME = room.WORKING_USERNAME;
            }
        }
    }
}
