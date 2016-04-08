namespace Fanex.NAP.Core.Test.Clients
{
    using Fanex.NAP.Core.Clients;
    using Fanex.NAP.Core.Models.ParameterObject;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Net.Http;

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
            UpdateUserPasswordParameter parameter = new UpdateUserPasswordParameter
            {
                CustId = 1,
                UserPwd = "1234",
            };

            _client.UpdatePassword(parameter);

            var expectedRequestUri = "http://10.18.200.209:1710/api/userinfo/updatePassword";
            Assert.AreEqual(expectedRequestUri, _respone.RequestMessage.RequestUri.ToString());
        }

        [Test]
        public void UpdateProfile_WhenCalled_RequestUri()
        {
            UpdateUserInfoParameter parameter = new UpdateUserInfoParameter
            {
                CustId = 1,
                FirstName = "name",
                LastName = "lastName",
                Email = "email@email.com"
            };

            _client.UpdateProfile(parameter);

            var expectedRequestUri = "http://10.18.200.209:1710/api/userinfo/updateProfile";
            Assert.AreEqual(expectedRequestUri, _respone.RequestMessage.RequestUri.ToString());
        }
    }
}