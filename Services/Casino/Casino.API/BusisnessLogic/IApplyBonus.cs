using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Context;
using System.Threading.Tasks;

namespace Casino.API.BusisnessLogic
{
    public interface IApplyBonus
    {
        Task<AccountTransaction> ApplyBonus(ApplicationDbContextBase dbContext, UserAccount userAccount);
    }
}
