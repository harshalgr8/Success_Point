using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessPointCore.Application.Interfaces;
using SucessPointCore.Api.Filters;
using SucessPointCore.Api.Helpers;
using SucessPointCore.Domain.Entities;

namespace SucessPointCore.Api.Controllers
{
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("api/CreateUser")]
        [Authorize]
        [ServiceFilter(typeof(JwtAuthenticationFilter))]
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


        [HttpGet]
        [Route("api/GetTotalUser")]
        [Authorize]
        ////[ServiceFilter(typeof(JwtAuthenticationFilter))]
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

        [HttpGet]
        [Route("api/GetStudents")]
        //[ServiceFilter(typeof(JwtAuthenticationFilter))]
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

        [HttpGet]
        [Route("api/GetEnrolledCourses")]
        //[ServiceFilter(typeof(JwtAuthenticationFilter))]
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

        private IEnumerable<string> GetModelStateErrors()
        {
            int errorNumber = 1;

            return ModelState.Values
                .SelectMany(state => state.Errors)
                .Select(error => $"{errorNumber++}. {error.ErrorMessage}");
        }
    }
}
