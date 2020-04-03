using System.Threading.Tasks;

namespace Casino.Services.Authentication.Contracts
{
    public interface IAwsCognitoUserGroups
    {
        Task AddUserToGroup(string Username, string GroupName);
    }
}
