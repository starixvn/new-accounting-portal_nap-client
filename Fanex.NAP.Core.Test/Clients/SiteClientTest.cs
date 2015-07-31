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
    public class SiteClientTest
    {
        private const string BaseEndpoint = "http://10.18.200.209:1710/api";
        private HttpResponseMessage _respone;
        private SiteClient _client;

        [SetUp]
        public void SetUp()
        {
            _respone = Substitute.For<HttpResponseMessage>();
            _respone.Content = Substitute.For<HttpContent>();
            _client = new SiteClient(new Uri(BaseEndpoint), new FakeDelegatingHandler(_respone));
        }

        [Test]
        public void IsActivated_WhenCalled_RequestUri()
        {
            var siteId = 1456;

            _client.IsActivated(siteId);

            var expectedRequestUri = "http://10.18.200.209:1710/api/site/1456/status";
            Assert.AreEqual(expectedRequestUri, _respone.RequestMessage.RequestUri.ToString());
        }

        [Test]
        public void GetUrlById_WhenCalled_RequestUri()
        {
            var siteId = 1456;
            var environment = "UAT";

            _client.GetUrlById(siteId, environment);

            var expectedRequestUri = "http://10.18.200.209:1710/api/site/1456/siteUrl?environment=UAT";
            Assert.AreEqual(expectedRequestUri, _respone.RequestMessage.RequestUri.ToString());
        }

        private Task<string> GetIsActivatedResult()
        {
            return Task.Factory.StartNew<string>(() => { return "A"; });
        }
    }
}
