﻿using Casino.Data.Models.Enums;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Queryable;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Casino.API.Components.Rounds
{
    public class OnlyGetRoulettesActiveStateQueryFilter : IQueryableFilter<Roulette>
    {
        private long _rouletteId = -1;

        public OnlyGetRoulettesActiveStateQueryFilter(long rouletteId)
        {
            _rouletteId = rouletteId;
        }

        public IQueryable<Roulette> SetFilter(IQueryable<Roulette> query)
        {
            return query
                .Include(x => x.Rounds)
                .Include(x => x.State)
                .Include(x => x.Type)
                .Where(x =>
                    x.Id.Equals(_rouletteId) &&
                    x.DeletedAt == null &&
                    x.State.Id == (long)RouletteStates.Active);
        }
    }
}
