using System;

namespace Casino.Services.Authentication.Contracts
{
    public interface ISignupModelUser
    {
        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
