namespace Fanex.NAP.Core.Models.ParameterObject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UpdatePasswordParameter
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string OldUserPwd { get; set; }

        public string UserPwd { get; set; }

        public string ConfirmUserPwd { get; set; }

        public string Token { get; set; }
    }
}
