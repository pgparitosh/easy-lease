using EasyLease.Model.Extensions;
using EasyLease.Model.Models;
using EasyLease.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EasyLease.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        readonly IErrorService _errorService;
        readonly IConfiguration _config;
        private const string CONTROLLER_NAME = "User";
        public UserController(IUserService userService, IConfiguration config, IErrorService errorService)
        {
            _userService = userService;
            _config = config;
            _errorService = errorService;
        }
        
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string userName, string password)
        {
            try
            {
                var user = _userService.ValidateLogin(userName, password);
                if(user.UserId > 0)
                {
                    var token = GenerateJWT(user);
                    return Ok(new { token });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch(Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "Login", 0, string.Concat(userName, ",", password), ex);
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("AddUser")]
        [Authorize]
        public long AddUser(User user)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _userService.AddUser(user, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "AddUser", userId, JsonConvert.SerializeObject(user), ex);
                return -1;
            }
        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        public User GetUserById(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _userService.GetUserById(id);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetUserById", userId, id.ToString(), ex);
                return new User();
            }
        }

        [HttpGet]
        [Route("GetUserByOrganizationId/{id}")]
        public IEnumerable<User> GetUserByOrganizationId(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _userService.GetUsersByOrganizationId(id);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetUsersByOrganizationId", userId, id.ToString(), ex);
                return new List<User>();
            }
        }

        [HttpPost]
        [Route("UpdateUser")]
        [Authorize]
        public User UpdateUser(User user)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _userService.UpdateUser(user, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "UpdateUser", userId, JsonConvert.SerializeObject(user), ex);
                return new User();
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize]
        public bool ChangePassword(long id, string password)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _userService.ChangePassword(id, password, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "ChangePassword", userId, string.Concat(id.ToString(),",",password), ex);
                return false;
            }
        }

        [HttpPost]
        [Route("InactivateUser/{id}")]
        [Authorize]
        public bool InactivateUser(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _userService.InActivateUser(id, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "InactivateUser", userId, Convert.ToString(id), ex);
                return false;
            }
        }

        [HttpPost]
        [Route("LockUser/{id}")]
        [Authorize]
        public bool LockUser(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _userService.LockUser(id, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "LockUser", userId, Convert.ToString(id), ex);
                return false;
            }
        }

        [HttpPost]
        [Route("UnlockUserUsingPhoneNo/{phone}")]
        [Authorize]
        public bool UnlockUserUsingPhoneNo(string phone)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _userService.UnlockUserUsingPhoneNo(phone, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "UnlockUserUsingPhoneNo", userId, Convert.ToString(phone), ex);
                return false;
            }
        }

        [HttpPost]
        [Route("UnlockUser/{id}")]
        [Authorize]
        public bool UnlockUser(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _userService.UnlockUser(id, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "UnlockUser", userId, Convert.ToString(id), ex);
                return false;
            }
        }

        [HttpPost]
        [Route("ResetPasswordToDefault/{id}")]
        [Authorize]
        public string ResetPasswordToDefault(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _userService.ResetPasswordToDefault(id, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "ResetPasswordToDefault", userId, Convert.ToString(id), ex);
                return "";
            }
        }

        private string GenerateJWT(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtAuth:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.PhoneNumber),
                new Claim("Org", userInfo.OrganizationId.ToString()),
                new Claim("Id", userInfo.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["JwtAuth:Issuer"],
              _config["JwtAuth:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
