using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary
{
    internal class InvalidReasonADO
    {
        public InvalidReasonADO() { }
        // Fields
        private string reasonEnLang;
        private string reasonVnLang;
        private string status;

        // Properties
        public string ReasonEnLang
        {
            get
            {
                return this.reasonEnLang;
            }
            set
            {
                this.reasonEnLang = value;
            }
        }

        public string ReasonVnLang
        {
            get
            {
                return this.reasonVnLang;
            }
            set
            {
                this.reasonVnLang = value;
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }
    }


}
