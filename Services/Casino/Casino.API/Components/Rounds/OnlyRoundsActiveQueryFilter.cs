using Casino.Data.Models.Enums;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Queryable;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Casino.API.Components.Rounds
{
    public class OnlyRoundsActiveQueryFilter : IQueryableFilter<Round>
    {
        public IQueryable<Round> SetFilter(IQueryable<Round> query)
        {
            return query
                .Include(x => x.State)
                .Include(x => x.Roulette.State)
                .Include(x => x.Roulette.Type)
                .Include(x => x.Roulette.Rules)
                    .ThenInclude(rules => rules.Type)
                .Where(x =>
                    x.DeletedAt == null &&
                    x.State.Id == (long)RoundStates.Opened);
        }
    }
}

