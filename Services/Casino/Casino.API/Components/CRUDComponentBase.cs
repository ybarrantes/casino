
using Casino.Services.DB.SQL.Contracts.CRUD;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Casino.Services.WebApi;
using AutoMapper;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.API.Services;
using Casino.Data.Models.Entities;

namespace Casino.API.Components
{
    public abstract class CRUDComponentBase<T> where T : class
    {
        private readonly IContextCRUD<T> _CRUD = null;
        private readonly IMapper _mapper;
        private readonly IIdentityApp<User> _identityApp;

        public string EntityClassName => typeof(T).Name;

        public IContextCRUD<T> CRUD => _CRUD;

        public IMapper Mapper => _mapper;

        public IIdentityApp<User> IdentityApp => _identityApp;

        public DbContext AppDbContext
        {
            get => CRUD.AppDbContext;
            set => CRUD.AppDbContext = value;
        }

        public CRUDComponentBase(IContextCRUD<T> dbContextCRUD, IMapper mapper, IIdentityApp<User> identityApp)
        {
            _CRUD = dbContextCRUD;
            _mapper = mapper;
            _identityApp = identityApp;
        }

        public abstract Task<ActionResult<WebApiResponse>> FindAllAndResponseAsync(int page, int recordsPerPage);

        public abstract Task<ActionResult<WebApiResponse>> FindAndResponseAsync(long id);

        public abstract Task<ActionResult<WebApiResponse>> CreateAndResponseFromEntityAsync(T entity);
        public abstract Task<ActionResult<WebApiResponse>> CreateAndResponseFromDTOAsync(IModelDTO dto);

        public abstract Task<ActionResult<WebApiResponse>> UpdateAndResponseFromEntityAsync(T entity);
        public abstract Task<ActionResult<WebApiResponse>> UpdateAndResponseFromDTOAsync(long id, IModelDTO dto);

        public abstract Task<ActionResult<WebApiResponse>> DeleteAndResponseAsync(long id);
        public abstract Task<ActionResult<WebApiResponse>> DeleteAndResponseFromEntityAsync(T entity);

        public ActionResult<WebApiResponse> MakeSuccessResponse(object data, string message)
        {
            return new WebApiResponse()
                .Success()
                .SetData(data)
                .SetMessage(message);
        }
    }
}
