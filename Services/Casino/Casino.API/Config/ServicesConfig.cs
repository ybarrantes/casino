using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Casino.API.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Casino.API.Filters;

namespace Casino.API.Config
{
    public class ServicesConfig : IConfig
    {

        private IServiceCollection services;
        private IConfiguration config;

        public ServicesConfig(IConfiguration config, IServiceCollection services)
        {
            this.config = config;
            this.services = services;
        }

        void IConfig.SetConfiguration()
        {
            AddControllers();

            AddDbContext();

            AddAuthentication();

            services.AddMvc();

            //var opts = config.GetAWSOptions();

            //services.AddDefaultAWSOptions(opts);
        }

        private void AddControllers()
        {
            services.AddControllers(
                    options =>options.Filters.Add(new HttpResponseExceptionFilter())
                )
                .AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        private void AddDbContext()
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            );
        }

        private void AddAuthentication()
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Audience = "26qsg5e071phpgoq2fcihaudqp";
                    options.Authority = "https://ruleta-casino-dev.auth.us-east-1.amazoncognito.com";
                });
        }
    }
}
