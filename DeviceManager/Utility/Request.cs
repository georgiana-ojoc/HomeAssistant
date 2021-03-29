using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace DeviceManager.Utility
{
    public class Request
    {
        public static HttpResponseMessage CreateResponse<T>(HttpStatusCode statusCode, T content)
        {
            return new HttpResponseMessage()
            {
                StatusCode = statusCode, Content = new StringContent(JsonConvert.SerializeObject(content))
            };
        }
    }
}