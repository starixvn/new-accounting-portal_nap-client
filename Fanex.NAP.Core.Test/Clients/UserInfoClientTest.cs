namespace Fanex.NAP.Core.Test.Clients
{
    using System;
    using System.Net.Http;
    using Core.Clients;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UserInfoClientTest
    {
        private const string BaseEndpoint = "http://10.18.200.209:1710/api";
        private HttpResponseMessage _respone;
        private UserInfoClient _client;
        private HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
            _respone = Substitute.For<HttpResponseMessage>();
            _respone.Content = Substitute.For<HttpContent>();
            _httpClient = HttpClientFactory.Create(new FakeDelegatingHandler(_respone));
            _client = new UserInfoClient(new Uri(BaseEndpoint), _httpClient);
        }

        [Test]
        public void UpdatePassword_WhenCalled_RequestUri()
        {
            int userId = 1;
            string password = "1234";
            var siteId = 1;

            _client.UpdatePassword(siteId, userId, password);

            var expectedRequestUri = "http://10.18.200.209:1710/api/userinfo/updatePassword?siteId=1&userId=1&password=1234";
            Assert.AreEqual(expectedRequestUri, _respone.RequestMessage.RequestUri.ToString());
        }
    }
}