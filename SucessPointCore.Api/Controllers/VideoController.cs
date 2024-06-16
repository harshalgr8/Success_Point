using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace SucessPointCore.Api.Controllers
{
    public class VideoController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("api/videos")]
        public IActionResult GetStreamVideo([FromQuery] string videoFileName)
        {
            // Validate the video file name
            if (string.IsNullOrEmpty(videoFileName))
            {
                var errorResponse = new
                {
                    isSuccess = false,
                    message = "invalid filename",
                    Details = new { }
                };

                return BadRequest(errorResponse);
            }

            // Get the full path to the video file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Videos", videoFileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                var errorResponse = new
                {
                    isSuccess = false,
                    message = "file not found",
                    Details = new { }
                };

                return NotFound(errorResponse);
            }

            // Open the file
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // Check if the request contains a Range header
            var rangeHeader = Request.Headers["Range"];
            if (!string.IsNullOrEmpty(rangeHeader) && RangeHeaderValue.TryParse(rangeHeader, out var range))
            {
                // Handle Range requests
                long start = range.Ranges.First().From ?? 0;
                long end = range.Ranges.First().To ?? fileStream.Length - 1;

                if (start < 0 || end >= fileStream.Length)
                {
                    // The requested range is invalid
                    return StatusCode((int)HttpStatusCode.RequestedRangeNotSatisfiable, "Invalid Range");
                }

                // Calculate the length of the content to send
                long contentLength = end - start + 1;

                // Create a partial content response
                Response.Headers.Add("Content-Range", $"bytes {start}-{end}/{fileStream.Length}");
                var partialContent = new FileStreamResult(fileStream, "video/mp4")
                {
                    FileDownloadName = videoFileName,
                    EnableRangeProcessing = true
                };
                partialContent.FileStream.Seek(start, SeekOrigin.Begin);
                return partialContent;
            }
            else
            {
                // No Range header, return the full content
                var fullContent = File(fileStream, "video/mp4");
                return fullContent;
            }
        }

    }


}
