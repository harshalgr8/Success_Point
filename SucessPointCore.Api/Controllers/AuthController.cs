using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessPointCore.Application.Interfaces;
using SucessPointCore.Api.Helpers;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Helpers;

namespace SucessPointCore.Api.Controllers
{
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        IUserService _userService;
        IErrorLogService _errorLogService;
        public AuthController(IUserService userService, IErrorLogService errorLogService)
        {
            _userService = userService;
            _errorLogService = errorLogService;
        }

        [HttpPost()]
        [Route("api/Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginUserRequest userinfo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userinfo.GrantType) || userinfo.GrantType != "password")
                {
                    var errorResponse = new { isSuccess = false, message = "invalid grant type", Details = new { } };
                    return BadRequest(errorResponse);
                }

                if (string.IsNullOrWhiteSpace(userinfo.UserName) || string.IsNullOrWhiteSpace(userinfo.Password))
                {
                    var errorResponse = new { isSuccess = false, message = "invalid credentials values", Details = new { } };
                    return BadRequest(errorResponse);
                }

                if (userinfo.UserName.Trim() == "createadminuser" && userinfo.Password == string.Format("adm1n{0}pwd", DateTime.Now.ToString("ddMMyyyy")) && _userService.GetUserCount() == 0)
                {
                    var newUserID = _userService.CreateUser(new CreateUser { UserName = "admin", Password = userinfo.Password, Active = true });


                    var errorResponse = new { isSuccess = false, message = "default User created", Details = new { } };
                    return Created("", errorResponse);
                }


                var authenticatedUser = _userService.CheckLoginCredentials(userinfo.UserName, userinfo.Password);
                if (authenticatedUser == null)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid Credentials", Details = new { } };
                    return Unauthorized(errorResponse);
                }

                var result = GetToken(new User { UserName = userinfo.UserName, UserID = authenticatedUser.UserID });
                _userService.UpsertRefreshToken(new UpsertRefreshToken { UserID = authenticatedUser.UserID, RefreshToken = result.RefreshToken });

                var okResponse = new { isSuccess = true, message = "login success", Details = new { usertype = authenticatedUser.UserType, token = result.Token, token_expires_in = AppConfigHelper.TokenExpiryMinute, refresh_token = result.RefreshToken } };

                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, "Internal Error Occured. Please Contact Us");
            }

        }

        private (string Token, Guid RefreshToken) GetToken(User userinfo)
        {
            return new JwtAuthManager(AppConfigHelper.JWTTokenEncryptionKey, AppConfigHelper.Issuer, AppConfigHelper.Audience).GenerateTokens(userinfo);
        }
    }
}
