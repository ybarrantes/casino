using Casino.API;
using Casino.Services.Authentication.Contracts;
using Casino.Test.IntegrationTest.Config.Mocks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Casino.Test.IntegrationTest.Helpers.Config
{
    public class ApiWebClient
    {
        private WebApplicationFactory<Startup> _factory = null;
        public HttpClient WebClient { get; }

        public ApiWebClient(bool useFakeServices = true)
        {
            _factory = new WebApplicationFactory<Startup>();

            WebClient = _factory.WithWebHostBuilder(builder =>
                {
                    if (useFakeServices)
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            services.AddScoped<IAuthentication, AuthenticationSuccessMock>();
                        });
                    }
                })
                .CreateClient();
        }
    }
}
