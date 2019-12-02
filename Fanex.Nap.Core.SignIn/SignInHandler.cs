using System.Web.Hosting;

namespace Fanex.NAP.Core.SignIn
{
    public static class SignInHandler
    {
        public static void Register()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedViewPathProvider());
        }
    }
}
