namespace Fanex.NAP.Core.Service.Interface
{
    using System.Web.Mvc;

    public interface SSOService
    {
        bool IsAuthenticated(string userId, string onuserId, string time, Controller controller);
    }
}