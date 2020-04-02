using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Casino.Services.DB.SQL.Contracts.Model
{
    public interface IModelDTOValidator<T> where T : class
    {
        Task<T> Validate(DbContext context);
    }
}
