using AutoMapper;
using Casino.Data.Models.Entities;
using Casino.Data.Models.DTO.Users;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Casino.Services.WebApi;
using Casino.Services.DB.SQL.Contracts;
using System;
using Casino.Data.Models.Default;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Casino.API.Components.Users
{
    public class UsersCrudComponent : SqlContextCrud<User>, IUserComponent
    {
        public UsersCrudComponent(IMapper mapper, IPagedRecords<User> pagedRecords)
            : base(mapper, pagedRecords)
        {
            ShowModelDTOType = typeof(UserShowDTO);
        }

        public async Task<ActionResult<WebApiResponse>> CreateUserAndUserAccountsAsync(UserSignUpDTO userDTO, string cloudIdentityId)
        {
            User userEntity = new User()
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                CloudIdentityId = cloudIdentityId,
            };

            ISqlTransaction transaction = ((ISqlTransaction)AppDbContext);

            await transaction.BeginTransactionAsync();

            User user = await CreateFromEntityAsync(userEntity);

            await CreateUserAccountsAsync(user);

            await transaction.CommitTransactionAsync();

            return new WebApiResponse().Success();
        }

        private async Task CreateUserAccountsAsync(User user)
        {
            UserAccountState userAccountActiveState = await AppDbContext
                .Set<UserAccountState>()
                .FirstOrDefaultAsync(x => x.Id.Equals((long)UserAccountStates.Active));

            List<UserAccountType> userAccountTypes = await AppDbContext
                .Set<UserAccountType>()
                .ToListAsync();

            foreach(UserAccountType accountType in userAccountTypes)
            {
                UserAccount userAccount = new UserAccount {
                    UserOwner = user, State = userAccountActiveState, Type = accountType };

                AppDbContext.Set<UserAccount>().Add(userAccount);
            }

            await AppDbContext.SaveChangesAsync();
        }
    }
}
