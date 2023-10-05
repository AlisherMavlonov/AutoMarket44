using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Helpers;
using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.Account;
using AutoMarket44.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AutoMarket44.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<Profile> profileRepository;
        private readonly IBaseRepository<User> userRepository;
        private readonly IBaseRepository<Basket> basketRepository;
        private readonly ILogger<AccountService> logger;

        public AccountService(IBaseRepository<Profile> _profile, IBaseRepository<User> _user, IBaseRepository<Basket> _basket,
                              ILogger<AccountService> _loger)
        {
            profileRepository = _profile;
            userRepository = _user;
            basketRepository = _basket;
            logger = _loger;
        }

        public async Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var user = await userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == model.UserName);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "пользователь не найден",
                        StatusCode = AutoMarket44.Domain.Enum.StatusCode.UserNotFound
                    };
                }
                user.Password = HashPasswordHelpers.HashPassword(model.NewPassword);
                await userRepository.Update(user);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "Пароль обнавлен",
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[ChangePassword]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == model.Name);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = AutoMarket44.Domain.Enum.StatusCode.UserNotFound
                    };
                }
                if (user.Password != HashPasswordHelpers.HashPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль или логин",

                    };
                }

                var result = Authenticate(user);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[Login]: {ex.Message}");

                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
        {
            try
            {
                var user = await userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == model.Name);
                if (user != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь с таким логином уже есть",
                    };
                }

                user = new User()
                {
                    Name = model.Name,
                    Role = AutoMarket44.Domain.Enum.Role.User,
                    Password = HashPasswordHelpers.HashPassword(model.Password)
                };

                await userRepository.Create(user);

                var profile = new Profile()
                {
                    UserId = user.Id,
                };

                await profileRepository.Create(profile);

                var basket = new Basket()
                {
                    UserId = user.Id
                };

                await basketRepository.Create(basket);

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[Register]: {ex.Message}");
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim (ClaimsIdentity.DefaultNameClaimType,user.Name),
                new Claim (ClaimsIdentity.DefaultRoleClaimType,user.Role.ToString())
            };

            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

    }
}