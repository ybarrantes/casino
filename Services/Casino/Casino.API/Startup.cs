using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Casino.API.Config;
using Microsoft.AspNetCore.Http;

namespace Casino.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ApiConfig.Singleton.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ApiConfig.Singleton.Services = services;
            ApiConfig.Singleton.ApplyServicesConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(ILoggerFactory loggerFactory, IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            ApiConfig.Singleton.App = app;
            ApiConfig.Singleton.Environment = env;
            ApiConfig.Singleton.HttpContextAccessor = httpContextAccessor;
            ApiConfig.Singleton.LoggerFactory = loggerFactory;
            ApiConfig.Singleton.ApplyAppConfiguracion();
        }
    }
}
