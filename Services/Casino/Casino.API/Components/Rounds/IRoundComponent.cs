using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Casino.API.Components.Rounds
{
    public interface IRoundComponent
    {
        Task<ActionResult<WebApiResponse>> GetAllRouletteRoundsPagedRecordsAsync(long rouletteId, int page);

        Task<ActionResult<WebApiResponse>> OpenRouletteRoundAsync(long rouletteId);

        Task<ActionResult<WebApiResponse>> CloseRouletteRoundAsync(long rouletteId, long roundId);
    }
}
