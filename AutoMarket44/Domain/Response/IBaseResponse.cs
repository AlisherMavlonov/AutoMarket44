using AutoMarket44.Domain.Enum;

namespace AutoMarket44.Domain.Response
{
    public interface IBaseResponse<T>
    {
        string Description { get; }
        StatusCode StatusCode { get; }
        T Data { get; }
    }
}
