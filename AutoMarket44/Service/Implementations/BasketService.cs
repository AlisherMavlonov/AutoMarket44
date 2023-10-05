using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Enum;
using AutoMarket44.Domain.Extensions;
using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.Order;
using AutoMarket44.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket44.Service.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly IBaseRepository<User> userRepository;
        private readonly IBaseRepository<Car> carRepository;

        public BasketService(IBaseRepository<User> _userRepository, IBaseRepository<Car> _carRepository)
        {
            userRepository = _userRepository;
            carRepository = _carRepository;
        }

        public async Task<IBaseResponse<OrderViewModel>> GetItem(string userName, long id)
        {
            try
            {
                var user = await userRepository.GetAll()
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Orders)
                    .FirstOrDefaultAsync(x => x.Name == userName);
                if (user == null)
                {
                    return new BaseResponse<OrderViewModel>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = AutoMarket44.Domain.Enum.StatusCode.UserNotFound
                    };
                }
                var orders = user.Basket?.Orders.Where(x => x.Id == id).ToList();
                if (orders == null || orders.Count == 0)
                {
                    return new BaseResponse<OrderViewModel>()
                    {
                        Description = "Заказов нет",
                        StatusCode = AutoMarket44.Domain.Enum.StatusCode.OrderNotFound
                    };
                }

                var response = (from p in orders
                                join c in carRepository.GetAll() on p.CarId equals c.Id
                                select new OrderViewModel()
                                {
                                    Id = p.Id,
                                    CarName = c.Name,
                                    Speed = c.Speed,
                                    TypeCar = c.TypeCar.GetDisplayName(),
                                    Model = c.Model,
                                    Address = p.Address,
                                    FirstName = p.FirstName,
                                    LastName = p.LastName,
                                    MiddleName = p.MiddleName,
                                    DateCreate = p.DateCreated.ToLongDateString(),
                                    Image = c.Avatar

                                }).FirstOrDefault();

                return new BaseResponse<OrderViewModel>()
                {
                    Data = response,
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<OrderViewModel>>> GetItems(string userName)
        {
            try
            {
                var user = await userRepository.GetAll()
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Orders)
                    .FirstOrDefaultAsync(x => x.Name == userName);

                if (user == null)
                {
                    return new BaseResponse<IEnumerable<OrderViewModel>>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var orders = user.Basket?.Orders;
                var response = (from p in orders
                                join c in carRepository.GetAll() on p.CarId equals c.Id
                                select new OrderViewModel()
                                {
                                    Id = p.Id,
                                    CarName = c.Name,
                                    Speed = c.Speed,
                                    TypeCar = c.TypeCar.GetDisplayName(),
                                    Model = c.Model,
                                    Image = c.Avatar
                                });

                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
