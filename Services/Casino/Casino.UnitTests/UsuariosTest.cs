using Casino.API.Controllers;
using Casino.API.Data.Context;
using Casino.API.Data.Models.Usuario;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Casino.API.Util.Response;
using Microsoft.AspNetCore.Mvc;
using Casino.API.Data.Entities;
using System.Collections.Generic;

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
                using (var dbContext = new ApplicationDbContext(optionsDbContext))
                {
                    dbContext.Usuarios.Add(new Usuario { Id = (long)1, Username = "ybarrantes", Email = "y@y.com", CloudIdentityId = "xxxx-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
                    dbContext.Usuarios.Add(new Usuario { Id = (long)2, Username = "ybarrantes2", Email = "y2@y.com", CloudIdentityId = "dddd-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
                    dbContext.Usuarios.Add(new Usuario { Id = (long)3, Username = "ybarrantes3", Email = "y3@y.com", CloudIdentityId = "mmmm-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });

                    dbContext.SaveChanges();
                }

                // change context
                using (var dbContext = new ApplicationDbContext(optionsDbContext))
                {
                    //UsuariosController usuarioController = new UsuariosController(dbContext);
                    //ActionResult<HttpResponse> response = await usuarioController.GetUsuario((long)1);
                    List<Usuario> usuario = await dbContext.Usuarios.ToListAsync<Usuario>();
                    Assert.AreEqual(3, usuario.Count);
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
                    dbContext.Usuarios.Add(new Usuario { Id = (long)4, Username = "ybarrantes", Email = "y4@y.com", CloudIdentityId = "iiii-aaaaa-bbbbbb-cc", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });

                    dbContext.SaveChanges();
                }

                // change context
                using (var dbContext = Helpers.GetNewDbContext())
                {
                    UsuariosController usuarioController = new UsuariosController(dbContext, Helpers.GetConfiguration(), null);
                    ActionResult<HttpResponse> response = await usuarioController.GetUsuario((long)1);

                    Assert.Pass();
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}