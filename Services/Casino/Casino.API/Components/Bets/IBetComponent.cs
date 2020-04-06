using Casino.Data.Models.DTO.Bets;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Casino.API.Components.Bets
{
    public interface IBetComponent
    {
        Task<ActionResult<WebApiResponse>> CreateBet(long roundId, BetCreateDTO betCreateDTO);

        Task<ActionResult<WebApiResponse>> CancelBet(long roundId, long betId);
    }
}
