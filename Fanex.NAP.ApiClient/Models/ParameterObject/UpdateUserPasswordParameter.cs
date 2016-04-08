namespace Fanex.NAP.Core.Models.ParameterObject
{
    public class UpdateUserPasswordParameter
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string UserPwd { get; set; }
    }
}
