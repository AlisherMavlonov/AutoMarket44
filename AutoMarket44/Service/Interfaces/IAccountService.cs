using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.Account;
using System.Security.Claims;

namespace AutoMarket44.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);

        Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model);
    }
}
