using System;
using System.Collections.Generic;
using System.Text;

namespace Casino.Services.DB.SQL.Contracts.Model
{
    public interface IEntityModelUser : IEntityModelBase
    {
        string Username { get; set; }

        string Email { get; set; }

        string CloudIdentityId { get; set; }
    }
}
