using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamTreatmentFinish.Base
{
    class Block : Button
    {
        public int GridX { get; set; }
        public int GridY { get; set; }

        public virtual void Remove()
        {
            this.Dispose();
        }
    }
}
