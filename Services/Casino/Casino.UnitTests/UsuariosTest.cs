using Casino.API.Controllers;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Casino.Data.Context;
using Casino.Services.WebApi;
using Casino.Data.Models.Entities;

namespace Casino.UnitTests
{
    public class UsuariosTests
    {
        private DbContextOptions<ApplicationDbContext> optionsDbContext;

        [SetUp]
        public void Setup()
        {
            optionsDbContext = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CasinoUnitTestDb")
                .Options;
        }

        [Test]
        public async Task SaveUsers()
        {
            try
            {
                // set context
                using (var dbContext = Helpers.GetNewDbContext())
                {
                    dbContext.Users.Add(new User { Username = "ybarrantes", Email = "y@y.com", CloudIdentityId = "xxxx-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
                    dbContext.Users.Add(new User { Username = "ybarrantes2", Email = "y2@y.com", CloudIdentityId = "dddd-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
                    dbContext.Users.Add(new User { Username = "ybarrantes3", Email = "y3@y.com", CloudIdentityId = "mmmm-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });

                    dbContext.SaveChanges();
                }

                // change context
                using (var dbContext = Helpers.GetNewDbContext())
                {
                    //UsuariosController usuarioController = new UsuariosController(dbContext);
                    //ActionResult<HttpResponse> response = await usuarioController.GetUsuario((long)1);
                    List<User> usuario = await dbContext.Users.ToListAsync<User>();

                    Assert.IsTrue(usuario.Count >= 3);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }            
        }


        [Test]
        public async Task FindUserById()
        {
            try
            {
                // set context
                using (var dbContext = Helpers.GetNewDbContext())
                {
                    dbContext.Users.Add(new User { Username = "ybarrantes", Email = "y4@y.com", CloudIdentityId = "iiii-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });

                    dbContext.SaveChanges();
                }

                // change context
                using (var dbContext = Helpers.GetNewDbContext())
                {
                    UsersController usuarioController = new UsersController(dbContext, Helpers.GetConfiguration(), null);
                    ActionResult<WebApiResponse> response = await usuarioController.GetUser((long)1);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            Assert.Pass();
        }
    }
}