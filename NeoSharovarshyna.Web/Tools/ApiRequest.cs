namespace NeoSharovarshyna.Web.Tools
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string? ApiUrl { get; set; }
        public object? ApiData { get; set; }
        public string? AccessToken { get; set; }
    }
}
