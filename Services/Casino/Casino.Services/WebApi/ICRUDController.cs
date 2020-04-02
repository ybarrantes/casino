
using Casino.Services.DB.SQL.Contracts.CRUD;
using Casino.Services.DB.SQL.Contracts.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Casino.Services.WebApi
{
    public interface ICRUDController<T> where T : class
    {
        IContextCRUD<T> CRUD { get; }

        DbContext AppDbContext { get; set; }

        Task<ActionResult<WebApiResponse>> FindAllAndResponseAsync(int page, int recordsPerPage, Order order = Order.ASC);

        Task<ActionResult<WebApiResponse>> FindAndResponseAsync(long id);

        Task<ActionResult<WebApiResponse>> CreateAndResponseAsync(T entity);

        Task<ActionResult<WebApiResponse>> UpdateAndResponseAsync(T entity);

        Task<ActionResult<WebApiResponse>> DeleteAndResponseAsync(long id);

        public virtual ActionResult<WebApiResponse> MakeSuccessResponse(object data, string message)
        {
            return new WebApiResponse()
                .Success()
                .SetData(data)
                .SetMessage(message);
        }

        public enum Order
        {
            ASC,
            DESC
        }
    }
}
