using AutoMapper;
using Casino.Data.Models.Entities;
using Casino.Data.Models.DTO.Users;
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
