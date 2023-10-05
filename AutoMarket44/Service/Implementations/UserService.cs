using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Enum;
using AutoMarket44.Domain.Extensions;
using AutoMarket44.Domain.Helpers;
using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.User;
using AutoMarket44.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket44.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Profile> _profileRepository;

        public UserService(IBaseRepository<User> baseRepository, ILogger<UserService> logger, IBaseRepository<Profile> profileRepositoryRepository)
        {
            _userRepository = baseRepository;
            _logger = logger;
            _profileRepository = profileRepositoryRepository;

        }

        public async Task<IBaseResponse<User>> Create(UserViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == model.Name);
                if (user != null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Пользователь с таким логином уже есть",
                        StatusCode = StatusCode.UserAlreadyExists
                    };
                }
                user = new User()
                {
                    Name = model.Name,
                    Role = Role.User,
                    Password = HashPasswordHelpers.HashPassword(model.Password),
                };

                await _userRepository.Create(user);

                var profile = new Profile()
                {
                    Address = string.Empty,
                    Age = 0,
                    UserId = user.Id,
                };

                await _profileRepository.Create(profile);

                return new BaseResponse<User>()
                {
                    Data = user,
                    Description = $"Пользователь добавлен!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UserService.Create] error:{ex.Message}");

                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренная ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteUser(long id)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _userRepository.Delete(user);
                _logger.LogInformation($"[UserService.DeleteUser] пользователь удален");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UserService.DeleteUser] error:{ex.Message}");
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренная ошибка: {ex.Message}"
                };
            }
        }

        public BaseResponse<Dictionary<int, string>> GetRoles()
        {
            try
            {
                var role = ((Role[])Enum.GetValues(typeof(Role)))
                    .ToDictionary(k => (int)k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = role,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UserService.GetRole] внутренная ошибка");

                return new BaseResponse<Dictionary<int, string>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<UserViewModel>>> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetAll()
                    .Select(x => new UserViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Role = x.Role.GetDisplayName()

                    }).ToListAsync();

                if (users.Count() <= 0)
                {
                    return new BaseResponse<IEnumerable<UserViewModel>>()
                    {
                        Description = $"В БД 0 элеменов",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                _logger.LogInformation($"[UserService.GetAll] получено элеменов {users.Count()}");

                return new BaseResponse<IEnumerable<UserViewModel>>()
                {
                    Data = users,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UserService.GetAll] внутренная ошибка");

                return new BaseResponse<IEnumerable<UserViewModel>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }
    }
}