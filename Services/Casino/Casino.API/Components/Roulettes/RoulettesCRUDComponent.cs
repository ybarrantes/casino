using AutoMapper;
using Casino.API.Services;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casino.API.Components.Roulettes
{
    public class RoulettesCrudComponent : SqlContextCrud<Roulette>
    {
        private readonly IIdentityApp<User> _identityApp;

        public RoulettesCrudComponent(
            IIdentityApp<User> identityApp,
            IMapper mapper,
            IPagedRecords<Roulette> pagedRecords)
            : base(mapper, pagedRecords)
        {
            QueryableFilter = new RoulettesQueryableFilter();

            _identityApp = identityApp;

            ShowModelDTOType = typeof(RouletteShowDTO);
        }

        public override async Task<Roulette> FillEntityFromModelDTO(Roulette entity, IModelDTO dto)
        {
            RouletteCreateDTO modelDto = (RouletteCreateDTO)dto;

            entity.Description = modelDto.Description;
            entity.State = await GetAndValidateRouletteProperties<RouletteState>(modelDto.State);
            entity.Type = await GetAndValidateRouletteProperties<RouletteType>(modelDto.Type);

            return entity;
        }

        public async Task<T> GetAndValidateRouletteProperties<T>(long id) where T : class
        {
            var entity = await AppDbContext
                .Set<T>()
                .FirstOrDefaultAsync(x => ((IEntityModelBase)x).Id.Equals(id));

            if (entity == null)
            {
                string entityClassName = typeof(T).Name;
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, $"{entityClassName} is invalid");
            }

            return entity;
        }

        protected override async Task OnBeforeCreate(Roulette entity)
        {
            entity.UserRegister = await _identityApp.GetUser(AppDbContext);
        }

        public override IPagedRecords<Roulette> MapPagedRecordsToModelDTO(IPagedRecords<Roulette> pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<RouletteShowDTO>>(pagedRecords.Result);

            return pagedRecords;
        }
    }
}
