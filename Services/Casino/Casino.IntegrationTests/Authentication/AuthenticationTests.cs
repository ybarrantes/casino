using Casino.API;
using Casino.Services.Authentication.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace Casino.IntegrationTests.Authentication
{
    [TestClass]
    public class AuthenticationTests
    {
        private WebApplicationFactory<Startup> _factory;

        [TestInitialize]
        public void Initialize()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        public WebApplicationFactory<Startup> GetWebHostBuilded()
        {
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IAuthentication, AuthenticationSuccessMock>();
                });
            });
        }

        [TestMethod]
        public async Task IfSigninSuccessfulReturnJson()
        {
            var client = GetWebHostBuilded().CreateClient();
            var url = "/api/auth/signin";

            var content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            Assert.AreEqual(expected: 200, actual: (int)response.StatusCode);
        }
    }
}
