
using AutoMapper;
using Casino.Services.DB.SQL.Contracts.CRUD;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.Util.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casino.Services.WebApi
{
    public class CRUDController<T> : ICRUDController<T> where T : class
    {
        private readonly IContextCRUD<T> _CRUD = null;
        private readonly IMapper _mapper;

        private string EntityClassName => typeof(T).Name;

        public CRUDController(IContextCRUD<T> dbContextCRUD, IMapper mapper)
        {
            _CRUD = dbContextCRUD;
            _mapper = mapper;
        }

        #region Implemented members

        public IContextCRUD<T> CRUD => _CRUD;

        public DbContext AppDbContext
        {
            get => CRUD.AppDbContext;
            set => CRUD.AppDbContext = value;
        }

        public async Task<ActionResult<WebApiResponse>> CreateAndResponseAsync(IModelDTO<T> modelDTO)
        {
            T entity = await CRUD.CreateFromDTOAsync(modelDTO);

            IModelDTO<T> mappedDTO = MapEntityToModelDTO(entity);

            return MakeSuccessResponse(mappedDTO, $"{EntityClassName} created successful");
        }

        public async Task<ActionResult<WebApiResponse>> DeleteAndResponseAsync(long id)
        {
            T entity = await CRUD.DeleteByIdAsync(id);

            IModelDTO<T> mappedDTO = MapEntityToModelDTO(entity);

            return MakeSuccessResponse(mappedDTO, $"{EntityClassName} deleted successful");
        }

        // TODO: pending add order pagination feature
        public async Task<ActionResult<WebApiResponse>> FindAllAndResponseAsync(int page, int recordsPerPage, ICRUDController<T>.Order order = ICRUDController<T>.Order.ASC)
        {
            IPagedRecords pagedRecords = await CRUD.FindAllPagedAsync(page, recordsPerPage);

            List<IModelDTO<T>> mappedListDTO = _mapper.Map<List<IModelDTO<T>>>(pagedRecords.Result);

            return MakeSuccessResponse(mappedListDTO, "");
        }

        public async Task<ActionResult<WebApiResponse>> FindAndResponseAsync(long id)
        {
            T entity = await CRUD.FindByIdAsync(id);

            IModelDTO<T> mappedDTO = MapEntityToModelDTO(entity);

            return MakeSuccessResponse(mappedDTO, "");
        }

        public async Task<ActionResult<WebApiResponse>> UpdateAndResponseAsync(long id, IModelDTO<T> modelDTO)
        {
            T entity = await CRUD.UpdateFromDTOAsync(id, modelDTO);

            IModelDTO<T> mappedDTO = MapEntityToModelDTO(entity);

            return MakeSuccessResponse(mappedDTO, $"{EntityClassName} updated successful");
        }

        #endregion

        private ActionResult<WebApiResponse> MakeSuccessResponse(object data, string message)
        {
            return new WebApiResponse()
                .Success()
                .SetData(data)
                .SetMessage(message);
        }

        protected virtual IModelDTO<T> MapEntityToModelDTO(T entity)
        {
            return _mapper.Map<IModelDTO<T>>(entity);
        }
    }
}
