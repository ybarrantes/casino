using AutoMapper;
using Casino.Data.Context;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.Model;
using System;
using Casino.Data.Models.DTO.Users;
using System.Threading.Tasks;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;

namespace Casino.API.Components.Users
{
    public class UsersCrudComponent : SqlContextCrud<User>
    {
        public UsersCrudComponent(IMapper mapper, IPagedRecords<User> pagedRecords)
            : base(mapper, pagedRecords)
        {
            ShowModelDTOType = typeof(UserShowDTO);
        }
    }
}
