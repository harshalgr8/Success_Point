using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessPointCore.Application.Custom_Filters;
using SuccessPointCore.Application.Interfaces;
using SuccessPointCore.Application.Services;
using SucessPointCore.Api.HttpHelper;
using SucessPointCore.Domain.Constants;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Entities.Requests;

namespace SucessPointCore.Api.Controllers
{
    public class CourseController : ControllerBase
    {
        ICourseService _courseService;
        IErrorLogService _errorLogService;
        public CourseController(ICourseService courseService, IErrorLogService errorLogService)
        {
            _courseService = courseService;
            _errorLogService = errorLogService;
        }

        [HttpGet]
        [Authorize]
        [Route("api/GetCourses")]
        [AuthUserType(1)]
        public IActionResult GetCourses()
        {
            try
            {

                var result = _courseService.GetCourses();

                var okResponse = new { isSuccess = true, message = "OK", Details = result };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, MessageConstant.InternalServerError);
            }
        }


        [HttpPost]
        [Authorize]
        [AuthUserType(1)]
        [Route("api/UpsertCourse")]
        public IActionResult CreateCourse([FromQuery] CreateCourseRequest createCourseRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid or Incomplete information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                int userID = HttpContextHelper.GetUserIDFromClaims(this);

                var result = _courseService.UpsertCourse(createCourseRequest.CourseID, createCourseRequest.CourseName, userID);

                var okResponse = new { isSuccess = true, message = "OK", Details = result };
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
                int userID = HttpContextHelper.GetUserIDFromClaims(this);

                var result = _courseService.GetEnrolledCourses(userID);

                var okResponse = new { isSuccess = true, message = "OK", Details = result };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, MessageConstant.InternalServerError);
            }
        }
    }
}
