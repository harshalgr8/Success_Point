using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessPointCore.Application.Interfaces;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Helpers;

namespace SucessPointCore.Api.Controllers
{
    public class AuthController : ControllerBase
    {
        IUserService _userService;
        IErrorLogService _errorLogService;
        public AuthController(IUserService userService, IErrorLogService errorLogService)
        {
            _userService = userService;
            _errorLogService = errorLogService;
        }

        /// <summary>
        /// this endpoint will be entry point for user. The token generated will decide further what action/endpoint user can perform.
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("api/Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginUserRequest userinfo)
        {
            try
            {
                var validationResult = _userService.ValidateLoginRequest(userinfo);
                if (!validationResult.isValid)
                {
                    return BadRequest(new { isSuccess = false, message = validationResult.message, Details = new { } });
                }

                if (_userService.ShouldCreateAdminUser(userinfo))
                {
                    _userService.CreateAdminUser(userinfo.Password);
                    return Created("", new { isSuccess = false, message = "default User created", Details = new { } });
                }

                var authenticatedUser = _userService.CheckLoginCredentials(userinfo.UserName, userinfo.Password);
                if (authenticatedUser == null)
                {
                    return Unauthorized(new { isSuccess = false, message = "Invalid Credentials", Details = new { } });
                }

                // JWT Token Expiry time
                var tokenExpiryTime = new DateTime().ToUniversalTime().AddMinutes(Convert.ToInt32(AppConfigHelper.TokenExpiryMinute));

                var tokenResult = _userService.GenerateToken(authenticatedUser);
                _userService.UpsertRefreshToken(new UpsertRefreshToken() { UserID = authenticatedUser.UserID, RefreshToken = tokenResult.RefreshToken, RefreshTokenExpiryTime = tokenExpiryTime });

                var okResponse = new
                {
                    isSuccess = true,
                    message = "login success",
                    Details = new
                    {
                        usertype = authenticatedUser.UserType,
                        token = tokenResult.Token,
                        token_expires_in = tokenExpiryTime,
                        refresh_token = tokenResult.RefreshToken
                    }
                };

                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, "Internal Error Occurred. Please Contact Us");
            }
        }
    }
}