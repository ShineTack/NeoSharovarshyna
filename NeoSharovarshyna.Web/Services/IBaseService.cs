using NeoSharovarshyna.Web.Tools;

namespace NeoSharovarshyna.Web.Services
{
    public interface IBaseService : IDisposable
    {
        ResponseDto ResponseDto { get; set; }
        Task<T> SendAsync<T>(ApiRequest request);
    }
}
