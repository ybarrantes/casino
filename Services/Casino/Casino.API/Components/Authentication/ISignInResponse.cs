using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Authentication
{
    interface ISignInResponse
    {
        public string AccessToken { get; }
        public string TokenType { get; }
        public string RefreshToken { get; }
        public string IdToken { get; }
        public int ExpiresIn { get; }
    }
}
