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

        public OrderService(IBaseRepository<Order> baseRepository, IBaseRepository<User> baseRepository1)
        {
            orderRepository = baseRepository;
            userRepository = baseRepository1;

        }

        public async Task<IBaseResponse<Order>> Create(CreateOrderViewModel model)
        {
            try
            {
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
                    DateCreated = DateTime.Now,
                    BasketId = user.Basket.Id,
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

        public Task<IBaseResponse<bool>> Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}
