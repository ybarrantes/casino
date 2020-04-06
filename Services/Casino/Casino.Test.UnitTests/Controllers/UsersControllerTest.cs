using Casino.API.Controllers;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Casino.Data.Context;
using Casino.Services.WebApi;
using Casino.Data.Models.Entities;
using Microsoft.Extensions.Configuration;
using Casino.Services.Authentication.Contracts;
using Casino.Test.UnitTests.Config.Mocks;
using Casino.Data.Models.DTO.Users;
using Casino.Services.DB.SQL.Crud;
using Casino.API.Components.Users;
using Casino.API.Components.UserAccounts;
using Casino.Services.Util.Collections;
using AutoMapper;
using Casino.API.Services;
using Casino.Data.Models.Views;

namespace Casino.Test.UnitTests.Controllers
{
    public class UsersControllerTests
    {
        private ApplicationDbContext _dbContext = null;
        private IConfiguration _configuration;
        private UsersController _controller = null;
        private IAwsCognitoUserGroups _cognitoUserGroups = null;
        private List<string> _authorizedRoles = new List<string> { };
        private ISqlContextCrud<User> _userCrudComponent = null;
        private ISqlContextCrud<UserAccount> _sqlContextCrudUserAccount = null;
        private ISqlContextCrud<UserAccountBalance> _sqlContextCrudUserAccountBalance = null;
        private IPagedRecords<User> _pagedRecords = new PagedRecords<User>();
        private IPagedRecords<UserAccount> _pagedRecordsUserAccount = new PagedRecords<UserAccount>();
        private IMapper _mapper = Helpers.GetAutoMapperConfiguration();
        
        private IIdentityApp<User> _identityApp = new IdentityAppMock();

        private bool initialized = false;

        [SetUp]
        public void Setup()
        {
            if (initialized) return;

            // create db context
            _dbContext = Helpers.GetNewDbContext("CasinoDbUsersControllerTest");
            _configuration = Helpers.GetConfiguration();
            _cognitoUserGroups = new AwsCognitoUserGroupsMock();
            _sqlContextCrudUserAccountBalance = new SqlContextCrud<UserAccountBalance>(_mapper, new PagedRecords<UserAccountBalance>());
            _sqlContextCrudUserAccount = new UserAccountsCrudComponent(
                _mapper,
                _pagedRecordsUserAccount,
                _identityApp,
                _sqlContextCrudUserAccountBalance,
                null);

            _userCrudComponent = new UsersCrudComponent(_mapper, _pagedRecords, _sqlContextCrudUserAccount);

            _controller = new UsersController(_dbContext, _configuration, _cognitoUserGroups, _userCrudComponent);

            _authorizedRoles = _configuration.GetSection("AWS:Cognito:AuthorizedGroups").Get<List<string>>();

            _dbContext.Users.Add(new User { Username = "ybarrantes", Email = "y@y.com", CloudIdentityId = "xxxx-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            _dbContext.Users.Add(new User { Username = "ybarrantes2", Email = "y2@y.com", CloudIdentityId = "dddd-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            _dbContext.Users.Add(new User { Username = "ybarrantes3", Email = "y3@y.com", CloudIdentityId = "mmmm-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });

            _dbContext.SaveChanges();

            initialized = true;
        }

        [Test]
        public async Task When_UsersFindById_OK()
        {
            try
            {
                for (long i = 1; i <= 3; i++)
                {
                    ActionResult<WebApiResponse> response = await _controller.GetUser(i);

                    Assert.IsNotNull(response);
                    Assert.AreEqual(200, response.Value.Status);
                    Assert.AreEqual(i, ((UserShowDTO)response.Value.Data).Id);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public async Task When_UsersFindById_NotFound()
        {
            try
            {
                ActionResult<WebApiResponse> response = await _controller.GetUser(99);

                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Pass(e.Message);
            }
        }

        [Test]
        public void When_CheckIfRoleAuthorized_OK()
        {
            CheckIfRoleIsAuthorized(_authorizedRoles, true);
        }

        [Test]
        public void When_CheckIfRoleAuthorized_Fail()
        {
            List<string> roles = new List<string> { "Avenger", "SuperHero", "Adventure" };

            CheckIfRoleIsAuthorized(roles, false);
        }

        private void CheckIfRoleIsAuthorized(List<string> roleList, bool assertPass)
        {
            foreach (string role in roleList)
            {
                bool result = _controller.CheckRoleIsAuthorized(role);

                Assert.AreEqual(assertPass, result, role);
            }
        }


        [Test]
        public async Task When_AddRole_OK()
        {
            foreach(string role in _authorizedRoles)
            {
                UserRoleDTO dto = new UserRoleDTO { Role = role };

                try
                {
                    ActionResult<WebApiResponse> result = await _controller.AddRole(1, dto);

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.Value.Status, role);
                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }
            }
        }

        [Test]
        public async Task When_AddRole_RoleNotAuthorized()
        {
            await AddRole_Fail("Avenger", 1);
        }

        [Test]
        public async Task When_AddRole_UserNotFound()
        {
            await AddRole_Fail("Player", 99);
        }

        private async Task AddRole_Fail(string role, long userId)
        {
            UserRoleDTO dto = new UserRoleDTO { Role = role };

            try
            {
                ActionResult<WebApiResponse> result = await _controller.AddRole(userId, dto);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Pass(e.Message);
            }
        }
    }
}