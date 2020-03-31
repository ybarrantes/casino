using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Casino.API.Data.Context
{
    public interface IContext
    {
        DbContext Context { get; }
        string ConnectionString { get; }
    }
}
