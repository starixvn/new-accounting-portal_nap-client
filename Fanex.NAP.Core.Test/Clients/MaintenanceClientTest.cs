namespace Fanex.NAP.Core.Test.Clients
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Models.ParameterObject;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MaintenanceClientTest
    {
        private const string BaseEndpoint = "http://10.18.200.209:1710/api";

        [Test]
        public void Logout_WhenCalled_RequestUri()
        {
            var respone = Substitute.For<HttpResponseMessage>();
            respone.Content = Substitute.For<HttpContent>();
            var client = new MaintenanceClient(new Uri(BaseEndpoint), new FakeDelegatingHandler(respone));

            client.GetDowntime();

            var expectedRequestUri = "http://10.18.200.209:1710/api/maintenance/";
            Assert.AreEqual(expectedRequestUri, respone.RequestMessage.RequestUri.ToString());
        }
    }
}
