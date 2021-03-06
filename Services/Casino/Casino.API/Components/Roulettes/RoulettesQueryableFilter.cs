﻿using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.DB.SQL.Queryable;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Casino.API.Components.Roulettes
{
    public class RoulettesQueryableFilter : IQueryableFilter<Roulette>
    {
        public IQueryable<Roulette> SetFilter(IQueryable<Roulette> query)
        {
            return query
                .Include(x => x.State)
                .Include(x => x.Type)
                .Where(x => x.DeletedAt == null);
        }
    }
}
