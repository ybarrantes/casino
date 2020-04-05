using AutoMapper;
using Casino.API.Components.Roulettes;
using Casino.API.Services;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.Entities;
using Casino.Services.Util.Collections;
using Casino.Test.UnitTests.Config.Mocks;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Test.UnitTests.Components.Roulettes
{
    class RoulettesComponentTest
    {
        private ApplicationDbContext _dbContext = null;
        private RoulettesCrudComponent _component = null;
        IIdentityApp<User> _identity = null;
        private IPagedRecords<Roulette> _pagedRecords = new PagedRecords<Roulette>();
        private IMapper _mapper = Helpers.GetAutoMapperConfiguration();

        private bool initialized = false;


        [SetUp]
        public async Task Setup()
        {
            if (initialized) return;

            // create db context
            _dbContext = Helpers.GetNewDbContext("CasinoDbRoulettesComponentsTest");

            _identity = new IdentityAppMock();

            _component = new RoulettesCrudComponent(_identity, _mapper, _pagedRecords);
            _component.AppDbContext = _dbContext;

            // add roulettes db dependencies
            RouletteState rouletteActiveState = new RouletteState{ State = "Active" };
            _dbContext.RouletteStates.Add(rouletteActiveState);
            RouletteState rouletteInactiveState = new RouletteState { State = "Inactive" };
            _dbContext.RouletteStates.Add(rouletteInactiveState);

            RouletteType rouletteEuropeanType = new RouletteType { Type = "European" };
            _dbContext.RouletteTypes.Add(rouletteEuropeanType);
            RouletteType rouletteAmercianType = new RouletteType { Type = "American" };
            _dbContext.RouletteTypes.Add(rouletteAmercianType);

            // add 4 rouletttes to db
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette A", State = rouletteActiveState, Type = rouletteEuropeanType });
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette B", State = rouletteActiveState, Type = rouletteEuropeanType });
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette C", State = rouletteActiveState, Type = rouletteAmercianType });
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette D", State = rouletteActiveState, Type = rouletteAmercianType });

            _dbContext.Roulettes.Add(new Roulette { Description = "roulette E", State = rouletteInactiveState, Type = rouletteEuropeanType });
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette F", State = rouletteInactiveState, Type = rouletteEuropeanType });
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette G", State = rouletteInactiveState, Type = rouletteAmercianType });
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette G", State = rouletteInactiveState, Type = rouletteAmercianType });

            _dbContext.Roulettes.Add(new Roulette { Description = "roulette YYY", State = rouletteActiveState, Type = rouletteEuropeanType, DeletedAt = DateTime.Now });
            _dbContext.Roulettes.Add(new Roulette { Description = "roulette ZZZ", State = rouletteActiveState, Type = rouletteEuropeanType, DeletedAt = DateTime.Now });

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

            Assert.AreEqual(8, count);
        }

        [Test]
        public async Task When_GetAndValidateRouletteProperties_Throw()
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
        public async Task When_GetAndValidateRouletteProperties_Found()
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
        public async Task When_FillEntityFromDTO_FillEntity()
        {
            try
            {
                Roulette roulette = await FillEntityFromDTO(1, 1);

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
        public async Task When_FillEntityFromDTO_NotFillEntity()
        {
            try
            {
                Roulette roulette = await FillEntityFromDTO(1, 10);

                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Pass();
            }
        }

        private async Task<Roulette> FillEntityFromDTO(long stateId, long typeId)
        {
            Roulette roulette = new Roulette();
            RouletteCreateDTO dto = new RouletteCreateDTO { Description = String.Empty, State = stateId, Type = typeId };

            return await _component.FillEntityFromModelDTO(roulette, dto);
        }

        [Test]
        public void When_MapPagedRecordsToModelDTO_Pass()
        {
            IPagedRecords<Roulette> pagedRecords = new PagedRecords<Roulette>();

            IQueryable<Roulette> queryable = _dbContext.Roulettes;

            int page = 1;
            int recordsPerPage = 5;

            pagedRecords.Build(queryable, page, recordsPerPage);

            // test

            try
            {
                _component.MapPagedRecordsToModelDTO(pagedRecords);

                Assert.AreEqual(page, pagedRecords.Page);
                Assert.AreEqual(recordsPerPage, pagedRecords.RecordsPerPage);

                foreach(var element in pagedRecords.Result)
                {
                    Assert.AreSame(typeof(RouletteShowDTO), element.GetType());
                    break;
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void When_RouletteRoundsConditionsOrAbort_Pass()
        {

        }
    }
}
