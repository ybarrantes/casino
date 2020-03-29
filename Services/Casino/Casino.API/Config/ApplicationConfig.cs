using Casino.API.Util.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Config
{
    public class ApplicationConfig : IConfig
    {
        private IConfiguration config;
        private IApplicationBuilder app;
        private IWebHostEnvironment env;
        private ILoggerFactory loggerFactory;

        public ApplicationConfig(IConfiguration config, IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            this.config = config;
            this.app = app;
            this.env = env;
            this.loggerFactory = loggerFactory;
        }

        void IConfig.SetConfiguration()
        {
            Logger.ConfigureLogger(loggerFactory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
