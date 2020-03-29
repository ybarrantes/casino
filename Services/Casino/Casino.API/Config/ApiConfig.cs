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




        public void ApplyServicesConfiguration()
        {
            if (serviceConfigApplied) throw new InvalidOperationException("The services configuration has already been applied");

            AddControllers();

            AddDbContext();

            AddAuthentication();

            AddAuthorization();

            Services.AddMvc();

            serviceConfigApplied = true;
        }

        private void AddControllers()
        {
            Services.AddControllers(
                    options => options.Filters.Add(new HttpResponseExceptionFilter())
                )
                .AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        private void AddDbContext()
        {
            Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
        }

        private void AddAuthentication()
        {
            string Region = Configuration["AWS:Cognito:Region"];
            string PoolId = Configuration["AWS:Cognito:PoolId"];
            string AppClientId = Configuration["AWS:Cognito:AppClientId"];
            string MetadataAddress = Configuration["AWS:Cognito:MetadataAddress"];

            // TODO: mejorar a singleton
            Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.Audience = AppClientId;
                    options.MetadataAddress = MetadataAddress;
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                    };
                });
        }

        private void AddAuthorization()
        {
            // TODO: mejorar a singleton
            Services
                .AddAuthorization(options =>
                {
                    //options.AddPolicy("SuperAdmin", policy => policy.require ("custom: groupId", new List<string> { "1" }));
                    options.AddPolicy("Admin", policy => policy.RequireClaim("cognito:groups", new List<string> { "Admin" }));
                    options.AddPolicy("Player", policy => policy.RequireClaim("cognito:groups", new List<string> { "Player" }));
                });
        }
    }
}
