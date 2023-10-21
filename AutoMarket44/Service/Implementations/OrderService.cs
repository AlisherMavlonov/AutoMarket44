using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Enum;
using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.Order;
using AutoMarket44.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket44.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Order> orderRepository;
        private readonly IBaseRepository<User> userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OrderService(IBaseRepository<Order> baseRepository, IBaseRepository<User> baseRepository1, IHttpContextAccessor _httpContextAccessor)
        {
            orderRepository = baseRepository;
            userRepository = baseRepository1;
            httpContextAccessor = _httpContextAccessor;
        }

        public async Task<IBaseResponse<Order>> Create(CreateOrderViewModel model)  
        {
            try
            {
                if (model.Login != httpContextAccessor.HttpContext.User.Identity.Name)
                {
                    return new BaseResponse<Order>()
                    {
                        Description = "Укажите свой логин для добовление заказа в карзину!!!",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                var user = await userRepository.GetAll()
                    .Include(x => x.Basket)
                    .FirstOrDefaultAsync(x => x.Name == model.Login);
                if (user == null)
                {
                    return new BaseResponse<Order>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var order = new Order()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Address = model.Address,
                    DateCreated = DateTime.Now.ToUniversalTime(),
                    BasketId = user.Basket.Id,
                    Basket = user.Basket,
                    CarId = model.CarId
                     
                };

                await orderRepository.Create(order);

                return new BaseResponse<Order>()
                {
                    Description = "Заказ создан",
                    Data = order,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Order>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(long id)
        {
            try
            {
                var order =await orderRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (order == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = $"B БД нет заказ с таким ID {id}",
                        StatusCode = StatusCode.OrderNotFound
                    };
                }

                var response = orderRepository.Delete(id);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = $"{order.FirstName} удалено из корзины",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
