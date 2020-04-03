using Casino.API.Components;
using Casino.API.Components.Roulettes;
using Casino.API.Services;
using Casino.Data.Context;
using Casino.Data.Models.DTO;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL;
using Casino.Services.DB.SQL.Contracts.CRUD;
using Casino.UnitTests.WebApi.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Casino.UnitTests.WebApi.Components.Roulettes
{
    class RoulettesComponentTest
    {
        private ApplicationDbContext _dbContext = null;
        private RoulettesCRUDComponent _component = null;
        ContextCRUD<Roulette> _crud = null;
        IIdentityApp<User> _identity = null;

        private bool initialized = false;


        [SetUp]
        public async Task Setup()
        {
            if (initialized) return;

            // create db context
            _dbContext = Helpers.GetNewDbContext("CasinoDbRoulettesComponentsTest");

            _crud = new ContextSqlCRUD<Roulette>();

            _identity = new IdentityAppMock();

            _component = new RoulettesCRUDComponent(_dbContext, _crud, _identity, null);

            // add roulettes db dependencies
            RouletteState rouletteState = new RouletteState{ State = "Active" };
            _dbContext.RouletteStates.Add(rouletteState);

            RouletteType rouletteType = new RouletteType { Type = "American" };
            _dbContext.RouletteTypes.Add(rouletteType);

            // add 4 rouletttes to db
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette 1", State = rouletteState, Type = rouletteType });
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette 2", State = rouletteState, Type = rouletteType });
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette 3", State = rouletteState, Type = rouletteType, DeletedAt = DateTime.Now });
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette 4", State = rouletteState, Type = rouletteType, DeletedAt = DateTime.Now });

            await _dbContext.SaveChangesAsync();

            initialized = true;
        }

        [Test]
        public void When_RoulettesQueryableFilter_AreEqual_2()
        {
            IQueryable<Roulette> queryable = _dbContext.Roulettes.AsQueryable();

            RoulettesQueryableFilter roulettesQueryableFilter = new RoulettesQueryableFilter();

            queryable = roulettesQueryableFilter.SetFilter(queryable);

            int count = queryable.Count();

            Assert.AreEqual(2, count);
        }

        [Test]
        public async Task When_RoulettesCRUDComponent_GetAndValidateRouletteProperties_Throw()
        {
            try
            {
                await _component.GetAndValidateRouletteProperties<RouletteState>(10);

                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Pass();
            }
        }

        [Test]
        public async Task When_RoulettesCRUDComponent_GetAndValidateRouletteProperties_Found()
        {
            try
            {
                RouletteState state = await _component.GetAndValidateRouletteProperties<RouletteState>(1);

                Assert.IsNotNull(state);
                Assert.AreEqual(1, state.Id);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task When_RoulettesCRUDComponent_SetIdentityUserToEntity_SetIdentity()
        {
            Roulette roulette = new Roulette();
            roulette = await _component.SetIdentityUserToEntity(roulette);

            Assert.IsNotNull(roulette.UserRegister);
            Assert.AreEqual(99999, roulette.UserRegister.Id);
        }

        [Test]
        public async Task When_RoulettesCRUDComponent_FillEntityFromDTO_FillEntity()
        {
            try
            {
                Roulette roulette = await RoulettesCRUDComponent_FillEntityFromDTO(1, 1);

                Assert.IsNotNull(roulette);
                Assert.IsNotNull(roulette.State);
                Assert.AreEqual(1, roulette.State.Id);
                Assert.IsNotNull(roulette.Type);
                Assert.AreEqual(1, roulette.Type.Id);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task When_RoulettesCRUDComponent_FillEntityFromDTO_NotFillEntity()
        {
            try
            {
                Roulette roulette = await RoulettesCRUDComponent_FillEntityFromDTO(1, 10);

                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Pass();
            }
        }

        private async Task<Roulette> RoulettesCRUDComponent_FillEntityFromDTO(long stateId, long typeId)
        {
            Roulette roulette = new Roulette();
            RouletteCreateDTO dto = new RouletteCreateDTO { Description = String.Empty, State = stateId, Type = typeId };

            return await _component.FillEntityFromDTO(roulette, dto);
        }
    }
}
