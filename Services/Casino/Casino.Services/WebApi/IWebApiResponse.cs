using System.Collections.Generic;

namespace Casino.Services.WebApi
{
    public interface IWebApiResponse
    {
        int Status { get; }
        string Title { get; }
        object Data { get;  }
        List<KeyValuePair<string, object>> Errors { get; }
    }
}
