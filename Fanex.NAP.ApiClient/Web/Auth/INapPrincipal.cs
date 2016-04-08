namespace Fanex.NAP.Core.Web.Auth
{
    public interface INapPrincipal
    {
        int UserId { get; set; }

        int OnUserId { get; set; }
    }
}