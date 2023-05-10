using JWTAuthentication.Database.Models;
using JWTAuthentication.Interfaces;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Identity;

namespace JWTAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public async Task<Response<UserModel>> GetUserById(int id)
        {
            var user = await _userManager.FindByIdAsync(Convert.ToString(id));

            if (user == null)
                return new Response<UserModel>()
                {
                    StatusCode = 404,
                    Message = "Not found!"
                };

            var viewUserModel = new UserModel()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return new Response<UserModel>()
            {
                StatusCode = 200,
                Result = viewUserModel
            };
        }

        public async Task<Response<UserModel>> GetUserByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);

            if (user == null)
                return new Response<UserModel>()
                {
                    StatusCode = 404,
                    Message = "Not found!"
                };

            var viewUserModel = new UserModel()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return new Response<UserModel>()
            {
                StatusCode = 200,
                Result = viewUserModel
            };
        }
    }
}
