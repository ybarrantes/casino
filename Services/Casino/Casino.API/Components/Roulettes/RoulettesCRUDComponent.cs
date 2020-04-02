using AutoMapper;
using Casino.API.Services;
using Casino.Data.Context;
using Casino.Data.Models.DTO;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.CRUD;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casino.API.Components.Roulettes
{
    public class RoulettesCRUDComponent : ICRUDComponent<Roulette>
    {
        public RoulettesCRUDComponent(ApplicationDbContext dbContext, IContextCRUD<Roulette> contextCRUD, IIdentityApp<User> identityApp, IMapper mapper)
            : base(dbContext, contextCRUD, identityApp, mapper)
        {
            RouletteQueryableFilter filter = new RouletteQueryableFilter();
            ContextCRUD.CustomFilter = filter;
        }

        public override async Task<Roulette> FillEntityFromDTO(Roulette entity, IModelDTO dto)
        {
            RouletteCreateDTO modelDto = (RouletteCreateDTO)dto;

            entity.Description = modelDto.Description;
            entity.State = await GetAndValidateRouletteProperties<RouletteState>(modelDto.State);
            entity.Type = await GetAndValidateRouletteProperties<RouletteType>(modelDto.Type);

            return entity;
        }

        private async Task<T> GetAndValidateRouletteProperties<T>(long id) where T : class
        {
            var entity = await ((ApplicationDbContext)ContextCRUD.AppDbContext)
                .Set<T>()
                .FirstOrDefaultAsync(x => ((IEntityModelBase)x).Id.Equals(id));

            if (entity == null)
            {
                string entityClassName = typeof(T).Name;
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, $"{entityClassName} is invalid");
            }

            return entity;
        }

        public override IModelDTO MapEntityToModelDTO(Roulette entity)
        {
            return Mapper.Map<RouletteShowDTO>(entity);
        }

        public override IPagedRecords MapPagedRecordsToModelDTO(IPagedRecords pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<RouletteShowDTO>>(pagedRecords.Result);

            return pagedRecords;
        }

        public override async Task<Roulette> SetIdentityUserToEntity(Roulette entity)
        {
            entity.UserRegister = await base.IdentityApp.GetUser(base.ContextCRUD.AppDbContext);

            return entity;
        }
    }
}
