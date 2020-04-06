using AutoMapper;
using Casino.Data.Models.Entities;
using Casino.Data.Models.DTO.Users;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Casino.Services.WebApi;
using Casino.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Casino.API.Components.Users
{
    public class UsersCrudComponent : SqlContextCrud<User>, IUserComponent
    {
        private ISqlContextCrud<UserAccount> _userAccountCrud = null;

        public UsersCrudComponent(
            IMapper mapper,
            IPagedRecords<User> pagedRecords,
            ISqlContextCrud<UserAccount> userAccountCrud)
            : base(mapper, pagedRecords)
        {
            ShowModelDTOType = typeof(UserShowDTO);

            _userAccountCrud = userAccountCrud;
        }

        public override IPagedRecords<User> MapPagedRecordsToModelDTO(IPagedRecords<User> pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<UserShowDTO>>(pagedRecords.Result);

            return pagedRecords;
        }

        public async Task<ActionResult<WebApiResponse>> CreateUserAndUserAccountsAsync(UserSignUpDTO userDTO, string cloudIdentityId)
        {
            User userEntity = new User()
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                CloudIdentityId = cloudIdentityId,
            };

            await AppDbContext.BeginTransactionAsync();

            User user = await CreateFromEntityAsync(userEntity);

            await CreateUserAccountsAsync(user);

            await AppDbContext.CommitTransactionAsync();

            return new WebApiResponse().Success();
        }

        private async Task CreateUserAccountsAsync(User user)
        {
            _userAccountCrud.AppDbContext = AppDbContext;

            UserAccountState userAccountActiveState = await AppDbContext
                .FindGenericElementByIdAsync<UserAccountState>((long)UserAccountStates.Active);

            List<UserAccountType> userAccountTypes = await AppDbContext
                .Set<UserAccountType>()
                .ToListAsync();

            foreach(UserAccountType accountType in userAccountTypes)
            {
                UserAccount userAccount = new UserAccount {
                    UserOwner = user, State = userAccountActiveState, Type = accountType };

                await _userAccountCrud.CreateFromEntityAsync(userAccount);
            }
        }
    }
}
