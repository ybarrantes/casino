using Casino.API.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Services
{
    public interface IApplicationDbContextService
    {
        ApplicationDbContext Context { get; }
    }
}
