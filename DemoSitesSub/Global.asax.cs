namespace DemoSitesSub
{
    using System.Web.Routing;
    using Fanex.Caching;
    using ONEbook.UM;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.NETFramework;
    using ONEbook.UM.Transport;


    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var umConfig = new UMClientConfiguration().Initialize();
            UMClientManager
                .UseConfig(umConfig)
                .UseCacheService(new UMCacheService(new CacheService()))
                .UseRestClient(new UMRestClient(umConfig))
                .UseHttpContext(new HttpContextHelper());

        }
    }
}