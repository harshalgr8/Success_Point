using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessPointCore.Application.Custom_Filters;
using SuccessPointCore.Application.Interfaces;
using SuccessPointCore.Api.HttpHelper;
using SuccessPointCore.Domain.Constants;
using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Entities.Requests;

namespace SuccessPointCore.Api.Controllers
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
        /// <param name="createUserRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/CreateUser")]
        [Authorize]
        [AuthUserType(1)]
        public IActionResult AddUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid user information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                var result = _userService.CreateUser(createUserRequest);

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
        public IActionResult CheckEmailExist([FromQuery] EmailAvailableForSignupRequest userEmailRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid email information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }


                var result = _userService.IsEmailAvailableForSignup(userEmailRequest.UserEmail);

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
        public IActionResult SignupUser([FromBody] SignupUserByEmailRequest userinfo)
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
        public IActionResult GetStudent([FromQuery] GetStudentListRequest studentRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid or Incomplete student information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                var result = _userService.GetStudentList(studentRequest.PageSize, studentRequest.PageNo, studentRequest.StudentName);
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
        [Route("api/UpsertStudent")]
        [Authorize]
        [AuthUserType(1)]
        public IActionResult UpsertStudent([FromBody] UpsertStudentRequest createUserRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid user information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                int userID = 0;
                if (createUserRequest.StudentID == 0)
                {
                    var insertedUserID = _userService.CreateUser(new CreateUserRequest
                    {
                        UserName = createUserRequest.UserName,
                        Password = createUserRequest.Password,
                        Active = createUserRequest.IsActive,
                        DisplayName = createUserRequest.StudentName,
                        UserType = 2
                    });

                    userID = insertedUserID;

                }

                var updatedUserID = _userService.UpdateStudentInfo(new UpdateStudentRequest
                {
                    UserID = userID,
                    StudentID = createUserRequest.StudentID,
                    UserName = createUserRequest.UserName,
                    Active = createUserRequest.IsActive,
                    DisplayName = createUserRequest.StudentName,
                    StandardID = createUserRequest.StandardID,
                    CourseID = createUserRequest.CourseID,
                    UserType = 2,
                    CourseIDCsv = createUserRequest.CourseIDCsv
                });

                var okResponse = new { isSuccess = true, message = "Student Upserted Successfully", Details = new { UserID = userID } };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Add User: {ex.Message}");
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                throw;
            }
        }

        /// <summary>
        /// This endpoint will called by admin to change password of particular user.
        /// </summary>
        /// <param name="objChangePassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ChangeStudentPassword")]
        [Authorize]
        [AuthUserType(1)]
        public IActionResult ChangePassword([FromBody] ChangeStudentPasswordRequest objChangePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid Student information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                _userService.ChangeStudentPassword(objChangePassword.StudentID, objChangePassword.Password);

                var okResponse = new { isSuccess = true, message = "Student Password Updated Successfully", Details = new { StudentID = objChangePassword.StudentID } };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Add User: {ex.Message}");
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                throw;
            }
        }

        /// <summary>
        /// This endpoint will called by admin to change password of particular user.
        /// </summary>
        /// <param name="objChangePassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ChangeUserPassword")]
        [Authorize]
        //[AuthUserType(1)]
        public IActionResult ChangeUserPassword([FromBody] ChangeUserPasswordRequest objChangePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid Student information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                int userID = HttpContextHelper.GetUserIDFromClaims(this);
                _userService.ChangeLoggedInUserPassword(userID, objChangePassword.Password);

                var okResponse = new { isSuccess = true, message = "User Password Changed Successfully", Details = new { UserID = userID } };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Add User: {ex.Message}");
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                throw;
            }
        }


        [HttpPost]
        [Route("api/RemoveStudent")]
        [Authorize]
        [AuthUserType(1)]
        public IActionResult RemoveStudent([FromBody] RemoveStudentRequest objRemoveStudent)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid Student information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                _userService.RemoveStudent(objRemoveStudent.StudentId);

                var okResponse = new { isSuccess = true, message = "Student Deleted Successfully", Details = new { } };
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Add User: {ex.Message}");
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                throw;
            }
        }
        #endregion


    }
}
