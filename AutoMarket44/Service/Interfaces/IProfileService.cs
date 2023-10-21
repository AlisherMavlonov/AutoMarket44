using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.Profile;

namespace AutoMarket44.Service.Interfaces
{
    public interface IProfileService
    {
        Task<BaseResponse<ProfileViewModel>> GetProfile(string userName);
        Task<BaseResponse<Profile>> Save(ProfileViewModel profile);

        //Task<BaseResponse<ProfileViewModel>> Edit(ProfileViewModel model);
    }
}
