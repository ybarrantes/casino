using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Casino.API.Data.Models.Roulette;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Roulettes
{
    public interface IRoulettesComponent
    {
        IQueryable<Roulette> QueryableRoulette(DbSet<Roulette> query);

        ApplicationDbContext AppDbContext { get; set; }

        Task<Roulette> FindRouletteById(int id);

        Task<Roulette> CreateRoulette(RouletteCreateDTO rouletteDTO);

        Task<Roulette> FillRouletteFromDTO(Roulette roulette, RouletteCreateDTO rouletteDTO);

        Task<Roulette> UpdateRouletteById(RouletteCreateDTO rouletteDTO, int id);

        Task<Roulette> DeleteRouletteById(int id);
    }
}
