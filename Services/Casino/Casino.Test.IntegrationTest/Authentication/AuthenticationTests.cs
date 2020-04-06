using Casino.Data.Models.DTO.Users;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Bogus;

namespace Casino.Test.IntegrationTest.Authentication
{
    public class AuthenticationTests
    {        

        public string SigninUrl => "/api/auth/signin";
        public string SignupUrl => "/api/auth/signup";
        public UserSignInDTO TestingCredentials => new UserSignInDTO
        {
            Username = "yonicristhb",
            Password = "123456Yb"
        };

        private HttpClient _webClient = Helpers.ApiHelper.GetRealWebClient().WebClient;
        private Faker _faker = Helpers.ApiHelper.GetFaker();


        [SetUp]
        public void Initialize()
        {
           
        }

        [Test]
        public async Task When_SignIn_Success()
        {
            var response = await Helpers.ApiHelper.SendPostRequestAndGetResponse<UserSignInDTO>(
                _webClient, SigninUrl, TestingCredentials);

            Assert.AreEqual(expected: 200, actual: (int)response.StatusCode);
        }
        
        [Test]
        public async Task When_SignIn_Fail()
        {
            UserSignInDTO fakeCredentials = new UserSignInDTO
            {
                Username = _faker.Person.UserName,
                Password = _faker.Internet.Password(8)
            };

            var response = await Helpers.ApiHelper.SendPostRequestAndGetResponse<UserSignInDTO>(
                _webClient, SigninUrl, fakeCredentials);

            Assert.AreEqual(expected: 401, actual: (int)response.StatusCode);
        }

        [Test]
        public async Task When_SignUp_Success()
        {
            UserSignUpDTO fakeData = new UserSignUpDTO
            {
                Username = _faker.Person.UserName,
                Name = _faker.Person.FirstName,
                MiddleName = _faker.Person.LastName,
                Email = _faker.Internet.Email(),
                Password = "Test1234"
            };

            var response = await Helpers.ApiHelper.SendPostRequestAndGetResponse<UserSignUpDTO>(
                _webClient, SignupUrl, fakeData);

            Assert.AreEqual(expected: 200, actual: (int)response.StatusCode);
        }
    }
}
