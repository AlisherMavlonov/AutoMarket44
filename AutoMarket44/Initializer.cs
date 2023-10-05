using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Dal.Repositories;
using AutoMarket44.Domain.Entity;
using AutoMarket44.Service.Implementations;
using AutoMarket44.Service.Interfaces;

namespace AutoMarket44
{
    public static class Initializer
    {
        public static void InitializerRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Car>, CarRepository>();
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IBaseRepository<Profile>, ProfileRepository>();
            services.AddScoped<IBaseRepository<Basket>, BasketRepository>();
            services.AddScoped<IBaseRepository<Order>, OrderRepository>();
        }



        public static void InitializerServices(this IServiceCollection services)
        {
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}
