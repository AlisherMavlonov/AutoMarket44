using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.Order;

namespace AutoMarket44.Service.Interfaces
{
    public interface IBasketService
    {
        Task<IBaseResponse<IEnumerable<OrderViewModel>>> GetItems(string userName);
        Task<IBaseResponse<OrderViewModel>> GetItem(string userName, long id);
    }
}
