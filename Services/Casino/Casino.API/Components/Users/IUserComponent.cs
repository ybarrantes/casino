using Casino.Data.Models.DTO.Users;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Casino.API.Components.Users
{
    public interface IUserComponent
    {
        Task<ActionResult<WebApiResponse>> CreateUserAndUserAccountsAsync(UserSignUpDTO userDTO, string cloudIdentityId);
    }
}
