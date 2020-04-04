using AutoMapper;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Rounds;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using Microsoft.AspNetCore.Http;

namespace Casino.API.Components.Rounds
{
    public class RoundsCrudComponent : SqlContextCrud<Round>
    {
        private readonly IHttpContextAccessor _httpContext;

        public RoundsCrudComponent(
            IMapper mapper,
            IPagedRecords<Round> pagedRecords,
            IHttpContextAccessor httpContext) : base(mapper, pagedRecords)
        {
            _httpContext = httpContext;
            ShowModelDTOType = typeof(RoundShowDTO);
        }
    }
}
