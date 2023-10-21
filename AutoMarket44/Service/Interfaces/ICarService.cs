using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.Car;

namespace AutoMarket44.Service.Interfaces
{
    public interface ICarService
    {
        BaseResponse<Dictionary<int, string>> GetTypes();
        Task<IBaseResponse<List<CarViewModel>>> GetCars();
        Task<IBaseResponse<CarViewModel>> GetCar(long Id);
        Task<BaseResponse<Dictionary<long, string>>> GetCar(string term);
        Task<IBaseResponse<Car>> Create(CarViewModel car);
        Task<IBaseResponse<bool>> DeleteCar(long id);
        Task<IBaseResponse<Car>> Edit(CarViewModel model);
    }
}
