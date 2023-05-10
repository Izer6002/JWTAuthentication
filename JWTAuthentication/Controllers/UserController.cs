using JWTAuthentication.Database.Models;
using JWTAuthentication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<Response<UserModel>> GetUserById(int id) => await _service.GetUserById(id);

        [Authorize]
        [HttpGet("[action]")]
        public async Task<Response<UserModel>> GetUserByName(string name) => await _service.GetUserByName(name);
    }
}

