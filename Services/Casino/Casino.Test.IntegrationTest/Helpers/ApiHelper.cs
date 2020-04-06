using Bogus;
using Casino.Test.IntegrationTest.Helpers.Config;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Casino.Test.IntegrationTest.Helpers
{
    public static class ApiHelper
    {
        private static ApiWebClient _realWebClient = null;
        private static ApiWebClient _fakeWebClient = null;

        private static Faker _faker = new Faker("es");        

        public static Faker GetFaker() => _faker;

        public static ApiWebClient GetRealWebClient()
        {
            if(_realWebClient == null)
                _realWebClient = new ApiWebClient(false);

            return _realWebClient;
        }

        public static ApiWebClient GetFakeWebClient()
        {
            if (_fakeWebClient == null)
                _fakeWebClient = new ApiWebClient();

            return _fakeWebClient;
        }

        public static async Task<HttpResponseMessage> SendGetRequestAndGetResponse<T>(HttpClient httpClient, string url)
        {
            return await httpClient.GetAsync(url);
        }

        public static async Task<HttpResponseMessage> SendPostRequestAndGetResponse<T>(HttpClient httpClient, string url, T data)
        {
            return await httpClient.PostAsync(url, GetJsonStringContentFromObject(data));
        }

        public static async Task<HttpResponseMessage> SendPutRequestAndGetResponse<T>(HttpClient httpClient, string url, T data)
        {
            return await httpClient.PutAsync(url, GetJsonStringContentFromObject(data));
        }

        public static async Task<HttpResponseMessage> SendDeleteRequestAndGetResponse<T>(HttpClient httpClient, string url)
        {
            return await httpClient.DeleteAsync(url);
        }

        public static StringContent GetJsonStringContentFromObject<T>(T data)
        {
            string dataJson = JsonSerializer.Serialize<T>(data);

            return new StringContent(
                dataJson, System.Text.Encoding.UTF8, "application/json");
        }
    }
}
