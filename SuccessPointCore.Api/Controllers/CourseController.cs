using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessPointCore.Application.Custom_Filters;
using SuccessPointCore.Application.Interfaces;
using SuccessPointCore.Application.Services;
using SuccessPointCore.Api.HttpHelper;
using SuccessPointCore.Domain.Constants;
using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Entities.Requests;

namespace SuccessPointCore.Api.Controllers
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
        [AuthUserType(1)]
        [Route("api/GetCourses")]
        public IActionResult GetCourses([FromQuery] GetCourseListRequest courseSearchRequest)
        {
            try
            {

                var result = _courseService.GetCourses();
                if (!string.IsNullOrWhiteSpace(courseSearchRequest?.Search))
                {
                    result = result.Where(x => x.CourseName.ToLower().Contains(courseSearchRequest?.Search.ToLower())).ToList();
                }

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
        public IActionResult CreateCourse([FromBody] CreateCourseRequest createCourseRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid or Incomplete information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                int userID = HttpContextHelper.GetUserIDFromClaims(this);

                var result = _courseService.UpsertCourse(createCourseRequest.CourseID, createCourseRequest.CourseName, createCourseRequest.IsActive, userID);

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
        [Route("api/UpsertVideoCourse")]
        public IActionResult CreateVideoCourse([FromBody] List<VideoCourse> createCourseRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid or Incomplete information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                int userID = HttpContextHelper.GetUserIDFromClaims(this);

                var result = _courseService.UpsertCourseVideo(createCourseRequest);

                var okResponse = new { isSuccess = true, message = "OK", Details = result };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, MessageConstant.InternalServerError);
            }
        }


        [HttpGet]
        [Authorize]
        [AuthUserType(1)]
        [Route("api/GetVideoCourse")]
        public IActionResult GetVideoCourse([FromQuery] int standardId, [FromQuery] int courseId)
        {
            try
            {

                var result = _courseService.GetVideoCourse(standardId, courseId);

                var okResponse = new { isSuccess = true, message = "OK", Details = result };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return StatusCode(500, MessageConstant.InternalServerError);
            }
        }

        [HttpGet]
        [Authorize]
        [AuthUserType(1)]
        [Route("api/GetUniqueVideoCourse")]
        public IActionResult GetUniqueVideoCourse([FromQuery] string Search)
        {
            try
            {
                var result = _courseService.GetUniqueVideoCourse();
                if (!string.IsNullOrWhiteSpace(Search))
                {
                    result = result.Where(x => x.CourseName.ToLower().Contains(Search.ToLower()) || x.StandardName.ToLower().Contains(Search.ToLower())).ToList();
                }

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

                var okResponse = new { isSuccess = true, message = "OK", Details = result?.ToList() };
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
        [Route("api/RemoveVideosForCourse")]
        public IActionResult CreateVideoCourse([FromBody] RemoveVideoCourseRequest removeCourseVideo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid or Incomplete information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                var result = _courseService.RemoveCourseVideo(removeCourseVideo);

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
