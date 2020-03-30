using Casino.API.Data.Context;
using Casino.API.Filters;
using Casino.API.Util.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Casino.API.Components.Authentication.AwsCognito;
using AutoMapper;
using Casino.API.Data.Entities;
using Casino.API.Data.Models.Ruleta;
using System.Security.Claims;
using Casino.API.Data.Models.Dominio;

namespace Casino.API.Config
{
    public class ApiConfig
    {
        private static ApiConfig _instance = null;
        private bool appConfigApplied = false;
        private bool serviceConfigApplied = false;

        public static ApiConfig Singleton
        {
            get
            {
                if (_instance == null) _instance = new ApiConfig();
                return _instance;
            }
        }

        public IConfiguration Configuration { get; set; }
        public IApplicationBuilder App { get; set; }
        public IWebHostEnvironment Environment { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }
        public IServiceCollection Services { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }


        /// <summary>
        /// Add application configuration
        /// </summary>
        public void ApplyAppConfiguracion()
        {
            if (appConfigApplied) throw new InvalidOperationException("The app configuration has already been applied");

            if (this.Environment.IsDevelopment())
            {
                //this.Environment.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/error");
            }

            App.UseHttpsRedirection();

            App.UseRouting();

            App.UseAuthentication();

            App.UseAuthorization();

            App.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            appConfigApplied = true;
        }

        /// <summary>
        /// Add services configuration
        /// </summary>
        public void ApplyServicesConfiguration()
        {
            if (serviceConfigApplied) throw new InvalidOperationException("The services configuration has already been applied");

            Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            AddControllersToServices();

            AddAutoMapperToServices();

            AddDbContextToServices();

            AddAuthenticationToServices();

            AddAuthorizationToServices();

            Services.AddMvc();

            serviceConfigApplied = true;
        }

        private void AddControllersToServices()
        {
            Services.AddControllers(
                    options => options.Filters.Add(new HttpResponseExceptionFilter())
                )
                .AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        private void AddAutoMapperToServices()
        {
            Services.AddAutoMapper(config =>
            {
                config.CreateMap<Ruleta, RuletaCreateDTO>();
                config.CreateMap<RuletaCreateDTO, Ruleta>();
                config.CreateMap<Ruleta, RuletaShowDTO>();
                //config.CreateMap<RuletaShowDTO, Ruleta>();

                config.CreateMap<Data.Entities.Dominio, DominioShowDTO>();
                config.CreateMap<DominioShowDTO, Data.Entities.Dominio>();
            }, typeof(Startup));
        }

        private void AddDbContextToServices()
        {
            Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
        }

        private void AddAuthenticationToServices()
        {
            // TODO: mejorar a singleton
            Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    IConfigJwtBearerAuthentication configJwtBearerAuthentication = new AwsCognitoConfigJwtBearerAuthentication(Configuration, null);
                    configJwtBearerAuthentication.GetJwtBearerAuthenticationOptions(options);
                });
        }

        private void AddAuthorizationToServices()
        {
            // TODO: mejorar a singleton
            Services
                .AddAuthorization(options =>
                {
                    IConfigAuthorization configAuthorization = new AwsCognitoConfigAuthorization(Configuration, null);
                    configAuthorization.GetAuthorizationOptions(options);
                });
        }
    }
}
