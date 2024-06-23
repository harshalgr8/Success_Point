using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessPointCore.Application.Custom_Filters;
using SuccessPointCore.Application.Interfaces;
using SucessPointCore.Api.Domain.Helpers;
using SucessPointCore.Domain.Entities;

namespace SucessPointCore.Api.Controllers
{
    public class UserController : ControllerBase
    {
        IUserService _userService;

        #region Construcor
        public UserController(IUserService userService)
        {
            _userService = userService;
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
            if (!ModelState.IsValid)
            {
                var errorResponse = new { isSuccess = false, message = "InvalidIncomplete user information provided", Details = GetModelStateErrors() };
                return BadRequest(errorResponse);
            }

            var result = _userService.CreateUser(userinfo);

            var okResponse = new { isSuccess = true, message = "User Created Successfully", Details = new { UserID = result } };
            return Ok(okResponse);
        }

        /// <summary>
        /// This enpoint will be used as dashboard showing how many user are registered with us
        /// Only admin and User will able to access it and result will vary based on Auth claims
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/GetTotalUser")]
        [Authorize]
        [AuthUserType(1,2)]
        public IActionResult GetTotalUser()
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = new { isSuccess = false, message = "InvalidIncomplete user information provided", Details = GetModelStateErrors() };
                return BadRequest(errorResponse);
            }

            var okResponse = new { isSuccess = true, message = "OK", Details = new { TotalUser = _userService.GetUserCount() } };
            return Ok(okResponse);
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
            if (!ModelState.IsValid)
            {
                var errorResponse = new { isSuccess = false, message = "InvalidIncomplete user information provided", Details = GetModelStateErrors() };
                return BadRequest(errorResponse);
            }

            var okResponse = new { isSuccess = true, message = "OK", Details = new { TotalUser = _userService.GetUserCount() } };
            return Ok(okResponse);
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
            if (!ModelState.IsValid)
            {
                var errorResponse = new { isSuccess = false, message = "InvalidIncomplete user information provided", Details = GetModelStateErrors() };
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

        #endregion

        #region Private functions

        private IEnumerable<string> GetModelStateErrors()
        {
            int errorNumber = 1;

            return ModelState.Values
                .SelectMany(state => state.Errors)
                .Select(error => $"{errorNumber++}. {error.ErrorMessage}");
        }

        #endregion

    }
}
