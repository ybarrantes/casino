using Microsoft.EntityFrameworkCore;

namespace Casino.Data.Context
{
    public interface IContext
    {
        DbContext Context { get; }
        string ConnectionString { get; }
    }
}
