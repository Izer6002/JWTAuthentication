using JWTAuthentication.Database.Models;

namespace JWTAuthentication.Interfaces
{
    public interface IUserService
    {
        Task<Response<UserModel>> GetUserById(int id);
        Task<Response<UserModel>> GetUserByName(string name);
    }
}
