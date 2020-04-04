using Microsoft.EntityFrameworkCore;

namespace Casino.Services.DB
{
    public interface IDbContext
    {
        DbContext AppDbContext { get; set; }
    }
}
