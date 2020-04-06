using Casino.Data.Models.Enums;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.DB.SQL.Queryable;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Casino.API.Components.Rounds
{
    public class GetRoundsOfARouletteQueryFilter : IQueryableFilter<Round>
    {
        private long _rouletteId = -1;

        public GetRoundsOfARouletteQueryFilter(long rouletteId = -1)
        {
            _rouletteId = rouletteId;
        }

        public IQueryable<Round> SetFilter(IQueryable<Round> query)
        {
            return query
                .Include(x => x.State)
                .Where(x =>
                    x.Roulette.Id.Equals(_rouletteId) &&
                    x.Roulette.DeletedAt == null &&
                    x.Roulette.State.Id == (long)RouletteStates.Active &&
                    x.DeletedAt == null)
                .OrderByDescending(x => x.Id);
        }
    }
}
