using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessPointCore.Application.Custom_Filters;
using SuccessPointCore.Application.Interfaces;
using SucessPointCore.Api.HttpHelper;
using SucessPointCore.Domain.Constants;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Entities.Requests;

namespace SucessPointCore.Api.Controllers
{
    public class StandardController : ControllerBase
    {
        IErrorLogService _errorLogService;
        IStandardService _standardService;
        public StandardController(IStandardService standardService, IErrorLogService errorLogService)
        {
            _standardService = standardService;
            _errorLogService = errorLogService;
        }

        [HttpGet]
        [Authorize]
        [Route("api/GetStandard")]
        public IActionResult GetStandard()
        {
            try
            {
                var result = _standardService.GetStandardList();

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
        [Route("api/UpsertStandard")]
        public IActionResult CreateStandard([FromQuery] CreateStandardRequest createStandardRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new { isSuccess = false, message = "Invalid or Incomplete information provided", Details = HttpContextHelper.GetModelStateErrors(this) };
                    return BadRequest(errorResponse);
                }

                int userID = HttpContextHelper.GetUserIDFromClaims(this);

                var result = _standardService.UpsertStandard(createStandardRequest.StandardID, createStandardRequest.StandardName, userID);

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
