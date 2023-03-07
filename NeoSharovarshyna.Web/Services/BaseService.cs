using NeoSharovarshyna.Web.Tools;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace NeoSharovarshyna.Web.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto ResponseDto { get; set; }
        private IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            ResponseDto = new ResponseDto();
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> SendAsync<T>(ApiRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ProductApi");
                var message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(client.BaseAddress + request.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                if (request.ApiData != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.ApiData), Encoding.UTF8, "application/json");
                }
                if (!string.IsNullOrEmpty(request.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);
                }
                switch (request.ApiType)
                {
                    case ApiType.POST: message.Method = HttpMethod.Post; break;
                    case ApiType.PUT: message.Method = HttpMethod.Put; break;
                    case ApiType.DELETE: message.Method = HttpMethod.Delete; break;
                    default: message.Method = HttpMethod.Get; break;
                }
                var response = await client.SendAsync(message);
                var apiContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(apiContent);
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto()
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string>() { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                return JsonConvert.DeserializeObject<T>(res);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
