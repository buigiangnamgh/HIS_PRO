using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary
{
    internal class VerifierADO
    {
        // Fields
        private string comment;
        private string location;
        private DateTime date;
        private List<InvalidReasonADO> invalidReasonList;
        private string isserDN;
        private string keyAlgorithm;
        private int keyLength;
        private bool modified;
        private DateTime notAfter;
        private DateTime notBefore;
        private string signerDN;
        private string signerLocation;
        private string signerName;
        private string signerOrganization;
        private string signerOrganizationUnit;
        private string signerSerialNumber;
        private bool valid;
        private System.Security.Cryptography.X509Certificates.X509Certificate2 x509Cert2;
        private Org.BouncyCastle.X509.X509Certificate x509CertBC;

        // Methods
        public VerifierADO(Org.BouncyCastle.X509.X509Certificate cert, System.Security.Cryptography.X509Certificates.X509Certificate2 cert2, string signerName, DateTime date, bool modified, string location)
        {
            this.x509Cert2 = cert2;
            this.x509CertBC = cert;
            this.signerName = signerName;
            this.signerOrganization = "";
            this.signerOrganizationUnit = "";
            this.signerLocation = location;
            this.modified = modified;
            this.valid = false;
            this.comment = "";
            this.date = date;
            this.keyLength = 0x400;
        }

        public SubjectDNADO SubjectDN { get; set; }

        // Properties
        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = value;
            }
        }

        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
            }
        }

        public List<InvalidReasonADO> InvalidReasonList
        {
            get
            {
                return this.invalidReasonList;
            }
            set
            {
                this.invalidReasonList = value;
            }
        }

        public string IsserDN
        {
            get
            {
                return this.isserDN;
            }
            set
            {
                this.isserDN = value;
            }
        }

        public string KeyAlgorithm
        {
            get
            {
                return this.keyAlgorithm;
            }
            set
            {
                this.keyAlgorithm = value;
            }
        }

        public int KeyLength
        {
            get
            {
                return this.keyLength;
            }
            set
            {
                this.keyLength = value;
            }
        }

        public bool Modified
        {
            get
            {
                return this.modified;
            }
            set
            {
                this.modified = value;
            }
        }

        public DateTime NotAfter
        {
            get
            {
                return this.notAfter;
            }
            set
            {
                this.notAfter = value;
            }
        }

        public DateTime NotBefore
        {
            get
            {
                return this.notBefore;
            }
            set
            {
                this.notBefore = value;
            }
        }

        public string SignerDN
        {
            get
            {
                return this.signerDN;
            }
            set
            {
                this.signerDN = value;
            }
        }

        public string SignerLocation
        {
            get
            {
                return this.signerLocation;
            }
            set
            {
                this.signerLocation = value;
            }
        }

        public string SignerName
        {
            get
            {
                return this.signerName;
            }
            set
            {
                this.signerName = value;
            }
        }

        public string SignerOrganization
        {
            get
            {
                return this.signerOrganization;
            }
            set
            {
                this.signerOrganization = value;
            }
        }

        public string SignerOrganizationUnit
        {
            get
            {
                return this.signerOrganizationUnit;
            }
            set
            {
                this.signerOrganizationUnit = value;
            }
        }

        public string SignerSerialNumber
        {
            get
            {
                return this.signerSerialNumber;
            }
            set
            {
                this.signerSerialNumber = value;
            }
        }

        public bool Valid
        {
            get
            {
                return this.valid;
            }
            set
            {
                this.valid = value;
            }
        }

        public System.Security.Cryptography.X509Certificates.X509Certificate2 X509Cert2
        {
            get
            {
                return this.x509Cert2;
            }
            set
            {
                this.x509Cert2 = value;
            }
        }

        public Org.BouncyCastle.X509.X509Certificate X509CertBC
        {
            get
            {
                return this.x509CertBC;
            }
            set
            {
                this.x509CertBC = value;
            }
        }
    }


}
