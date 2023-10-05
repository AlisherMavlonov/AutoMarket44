using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.User;

namespace AutoMarket44.Service.Interfaces
{
    public interface IUserService
    {
        Task<IBaseResponse<User>> Create(UserViewModel user);

        BaseResponse<Dictionary<int, string>> GetRoles();

        Task<BaseResponse<IEnumerable<UserViewModel>>> GetUsers();

        Task<IBaseResponse<bool>> DeleteUser(long id);
    }
}
