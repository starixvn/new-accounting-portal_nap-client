namespace Fanex.NAP.Core.Clients
{
    using System;
    using Fanex.NAP.Core.Web.Handler;

    internal sealed class NAPClients
    {
        internal NAPClients()
        {
        }

        public Func<NAPDelegatingHandler> CreateDelegatingHandler { get; set; }
    }
}
