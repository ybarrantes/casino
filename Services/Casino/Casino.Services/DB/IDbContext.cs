using Casino.Services.DB.SQL.Context;
using Microsoft.EntityFrameworkCore;

namespace Casino.Services.DB
{
    public interface IDbContext
    {
        ApplicationDbContextBase AppDbContext { get; set; }
    }
}
