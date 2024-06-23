using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessPointCore.Application.Custom_Filters;
using SuccessPointCore.Application.Interfaces;
using SuccessPointCore.Application.Services;
using SucessPointCore.Api.Domain.Helpers;
using SucessPointCore.Api.HttpHelper;
using SucessPointCore.Domain.Constants;
using SucessPointCore.Domain.Entities;

namespace SucessPointCore.Api.Controllers
{
    public class UserController : ControllerBase
    {
        IUserService _userService;
        IErrorLogService _errorLogService;
        #region Construcor
        public UserController(IUserService userService, IErrorLogService errorLogService)
        {
            _userService = userService;
            _errorLogService = errorLogService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// This enpoint will be called by Admin to create enrolled student account manually.
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/CreateUser")]
        [Authorize]
        [AuthUserType(1)]
        public IActionResult AddUser([FromBody] CreateUser userinfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid user information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                var result = _userService.CreateUser(userinfo);

                var okResponse = new { isSuccess = true, message = "User Created Successfully", Details = new { UserID = result } };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Add User: {ex.Message}");
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                throw;
            }
        }

        [HttpGet]
        [Route("api/IsEmailAvaliableForSignup")]
        [AllowAnonymous]
        public IActionResult CheckEmailExist([FromQuery] string userEmailID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid user information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }


                var result = _userService.IsEmailAvailableForSignup(userEmailID);

                var okResponse = new { isSuccess = true, message = "Ok", Details = new { CanRegister = result } };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, MessageConstant.InternalServerError);
            }
        }


        [HttpPost]
        [Route("api/SignupUser")]
        [AllowAnonymous]
        public IActionResult SignupUser([FromBody] SignupUserByEmail userinfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid user information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                if (!_userService.IsEmailAvailableForSignup(userinfo.EmailID))
                {
                    var errorResponse = new { isSuccess = false, message = MessageConstant.InvalidEmailIDForRegister, Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                var result = _userService.RegisterUserBySignup(new SignupCredentials() { EmailID = userinfo.EmailID, Password = userinfo.Password });

                var okResponse = new { isSuccess = true, message = MessageConstant.SignupVerificationEmailSent, Details = new { VID = result } };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, MessageConstant.InternalServerError);
            }

        }

        /// <summary>
        /// This enpoint will be used as dashboard showing how many user are registered with us
        /// Only admin and User will able to access it and result will vary based on Auth claims
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/GetTotalUser")]
        [Authorize]
        [AuthUserType(1, 2)]
        public IActionResult GetTotalUser()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "InvalidIncomplete user information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                var okResponse = new { isSuccess = true, message = "OK", Details = new { TotalUser = _userService.GetUserCount() } };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, MessageConstant.InternalServerError);
            }
        }

        /// <summary>
        /// This endpoint will return all students basic information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/GetStudents")]
        [Authorize]
        [AuthUserType(1)]
        public IActionResult GetStudent()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "InvalidIncomplete user information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                var okResponse = new { isSuccess = true, message = "OK", Details = new { TotalUser = _userService.GetUserCount() } };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, MessageConstant.InternalServerError);
            }
        }

        /// <summary>
        /// This endpoint will return logged in student enrolled course information list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/GetEnrolledCourses")]
        [AuthUserType(2)]
        public IActionResult GetEnrolledCourses()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "InvalidIncomplete user information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }


                var authorization = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
                {
                    // Handle missing or invalid Authorization header
                    return Unauthorized();
                }

                var token = authorization.Substring("Bearer ".Length).Trim();

                var claims = JwtAuthManager.GetPrincipal(token);

                var result = _userService.GetEnrolledCourses(Convert.ToInt32(claims.FindFirst("Uid").Value));

                var okResponse = new { isSuccess = true, message = "OK", Details = result };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, MessageConstant.InternalServerError);
            }
        }

        #endregion


    }
}
