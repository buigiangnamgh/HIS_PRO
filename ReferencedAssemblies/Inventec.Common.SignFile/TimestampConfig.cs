
namespace Inventec.Common.SignFile
{
    public class TimestampConfig
    {
        // Fields
        private string tsa_acc;
        private string tsa_pass;
        private string tsa_url = "";
        private bool useTimestamp;

        // Properties
        public string TsaAcc
        {
            get
            {
                return this.tsa_acc;
            }
            set
            {
                this.tsa_acc = value;
            }
        }

        public string TsaPass
        {
            get
            {
                return this.tsa_pass;
            }
            set
            {
                this.tsa_pass = value;
            }
        }

        public string TsaUrl
        {
            get
            {
                return this.tsa_url;
            }
            set
            {
                this.tsa_url = value;
            }
        }

        public bool UseTimestamp
        {
            get
            {
                return this.useTimestamp;
            }
            set
            {
                this.useTimestamp = value;
            }
        }
    }


}
