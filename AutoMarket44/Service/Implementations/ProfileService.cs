using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.Profile;
using AutoMarket44.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket44.Service.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly ILogger<ProfileService> _logger;
        private readonly IBaseRepository<Profile> profileRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBaseRepository<User> userRepository;

        public ProfileService(ILogger<ProfileService> logger, IBaseRepository<Profile> baseRepository,
               IHttpContextAccessor _httpContextAccessor,IBaseRepository<User> _userRepository)
        {
            _logger = logger;
            profileRepository = baseRepository;
            httpContextAccessor = _httpContextAccessor;
            userRepository = _userRepository;
        }

        public async Task<BaseResponse<ProfileViewModel>> GetProfile(string userName)
        {
            try
            {
                var profile = await profileRepository.GetAll().Select(x => new ProfileViewModel()
                {
                    Id = x.Id,
                    Address = x.Address,
                    Age = x.Age,
                    UserName = x.User.Name
                }).FirstOrDefaultAsync(x => x.UserName == userName);

                return new BaseResponse<ProfileViewModel>()
                {
                    Data = profile,
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"[ProfileService.GetProfile] error: {ex.Message}");

                return new BaseResponse<ProfileViewModel>()
                {
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.InternalServerError,
                    Description = $"Внутренная ошибка:{ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<Profile>> Save(ProfileViewModel model)
        {
            try
            {
                var profile = new Profile();
                var user = await userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == httpContextAccessor
                                                                            .HttpContext.User.Identity.Name);
                profile.UserId = user.Id;
                profile.User = user;
                profile.Address = model.Address;
                profile.Age = model.Age;

                await profileRepository.Update(profile);

                return new BaseResponse<Profile>()
                {
                    Data = profile,
                    Description = $"Данные обнавлены",
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"[ProfileSerice.Get] error:{ex.Message}");

                return new BaseResponse<Profile>()
                {
                    Description = $"Внутренная ошибка: {ex.Message}",
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        //public async Task<BaseResponse<ProfileViewModel>> Edit(ProfileViewModel model)
        //{
        //    try
        //    {
        //        var user = httpContextAccessor.HttpContext.User.Identity.Name;
        //        if (model.UserName == user)
        //        {
        //            var profileUser = await profileRepository.GetAll().FirstOrDefaultAsync(x => x.User.Name == user);
        //            profileUser.Address = model.Address;
        //            profileUser.Age = model.Age;

        //        }

                


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }




        //}

    }
}
