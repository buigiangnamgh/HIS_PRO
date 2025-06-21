using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.Integrate
{
    public class TokenData
    {
        public TokenData() { }

        public DateTime ExpireTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public string LoginAddress { get; set; }
        public DateTime LoginTime { get; set; }
        public string MachineName { get; set; }
        public string RenewCode { get; set; }
        public string TokenCode { get; set; }
        public UserData User { get; set; }
        public string VersionApp { get; set; }
    }

    public class UserData
    {
        public UserData() { }

        public string ApplicationCode { get; set; }
        public string Email { get; set; }
        public string GCode { get; set; }
        public string LoginName { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
    }
}
