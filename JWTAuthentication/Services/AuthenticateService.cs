using JWTAuthentication.Database.Models;
using JWTAuthentication.Database.Roles;
using JWTAuthentication.Interfaces;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<Response<LogInResponse>> LogIn(LogInModel logInModel)
        {
            var user = await _userManager.FindByNameAsync(logInModel.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, logInModel.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new Response<LogInResponse>()
                {
                    StatusCode = 200,
                    Result = new LogInResponse()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    }
                };
            }

            return new Response<LogInResponse>()
            {
                StatusCode = 401,
                Message = "Unauthorized"
            };
        }

        public async Task<Response<bool>> Register(RegisterModel registerModel)
        {
            var userExists = await _userManager.FindByNameAsync(registerModel.UserName);

            if (userExists != null)
                return new Response<bool>()
                {
                    StatusCode = 403,
                    Message = "User already exists!",
                    Result = false
                };

            var applicationUser = new ApplicationUser()
            {
                UserName = registerModel.UserName,
                Email = registerModel.EmailAddress,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(applicationUser, registerModel.Password);

            if (!result.Succeeded)
                return new Response<bool>()
                {
                    StatusCode = 424,
                    Message = "Failed to create user!",
                    Result = false
                };

            return new Response<bool>()
            {
                StatusCode = 200,
                Result = true
            };
        }

        public async Task<Response<bool>> RegisterAdmin(RegisterModel registerModel)
        {
            var userExists = await _userManager.FindByNameAsync(registerModel.UserName);

            if (userExists != null)
                return new Response<bool>()
                {
                    StatusCode = 403,
                    Message = "User already exists!",
                    Result = false
                };

            var applicationUser = new ApplicationUser()
            {
                UserName = registerModel.UserName,
                Email = registerModel.EmailAdress,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(applicationUser, registerModel.Password);

            if (!result.Succeeded)
                return new Response<bool>()
                {
                    StatusCode = 424,
                    Message = "Failed to create user!",
                    Result = false
                };

            if (!await _roleManager.RoleExistsAsync(Enum.GetName(UserRoles.Admin)))
                await _roleManager.CreateAsync(new IdentityRole(Enum.GetName(UserRoles.Admin)));

            if (!await _roleManager.RoleExistsAsync(Enum.GetName(UserRoles.User)))
                await _roleManager.CreateAsync(new IdentityRole(Enum.GetName(UserRoles.User)));

            if (await _roleManager.RoleExistsAsync(Enum.GetName(UserRoles.Admin)))
                await _userManager.AddToRoleAsync(applicationUser, Enum.GetName(UserRoles.Admin));

            return new Response<bool>()
            {
                StatusCode = 200,
                Result = true
            };
        }
    }
}
