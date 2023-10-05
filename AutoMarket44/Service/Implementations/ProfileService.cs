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

        public ProfileService(ILogger<ProfileService> logger, IBaseRepository<Profile> baseRepository)
        {
            _logger = logger;
            profileRepository = baseRepository;
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
                var profile = await profileRepository.GetAll().FirstOrDefaultAsync(x => x.Id == model.Id);
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
                _logger.LogInformation(ex, $"[ProfileSerice.Save] error:{ex.Message}");

                return new BaseResponse<Profile>()
                {
                    Description = $"Внутренная ошибка: {ex.Message}",
                    StatusCode = AutoMarket44.Domain.Enum.StatusCode.InternalServerError
                };
            }
        }
    }
}
