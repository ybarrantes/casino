using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Config
{
    interface IConfigJwtBearerAuthentication
    {
        public JwtBearerOptions GetJwtBearerAuthenticationOptions(JwtBearerOptions options);
    }
}
