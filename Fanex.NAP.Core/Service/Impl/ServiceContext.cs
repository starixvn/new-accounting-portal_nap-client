namespace Fanex.NAP.Core.Service.Impl
{
    using Fanex.NAP.Core.Service.Interface;

    public class ServiceContext : IServiceContext
    {
        public int UserId { get; set; }

        public int SiteId { get; set; }

        public string Ip { get; set; }
    }
}