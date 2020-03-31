using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Casino.API.Data.Models.Ruleta;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Ruletas
{
    public interface IRuletasComponent
    {
        IQueryable<Ruleta> QueryableRuleta(DbSet<Ruleta> query);

        ApplicationDbContext AppDbContext { get; set; }

        Task<Ruleta> FindRuletaById(int id);

        Task<Ruleta> CreateRuleta(RuletaCreateDTO ruletaDTO);

        Task<Ruleta> FillRuletaFromDTO(Ruleta ruleta, RuletaCreateDTO ruletaDTO);

        Task<Ruleta> UpdateRuletaById(RuletaCreateDTO ruletaDTO, int id);

        Task<Ruleta> DeleteRuletaById(int id);

        Task TrySaveRuleta(Ruleta ruleta, EntityState entityOperation);
    }
}
