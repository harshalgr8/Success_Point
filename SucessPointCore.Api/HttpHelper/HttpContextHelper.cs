using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SucessPointCore.Api.HttpHelper
{
    public class HttpContextHelper
    {

        public static IEnumerable<string> GetModelStateErrors(ControllerBase controller)
        {
            int errorNumber = 1;

            return controller.ModelState.Values
                .SelectMany(state => state.Errors)
                .Select(error => $"{errorNumber++}. {error.ErrorMessage}");
        }

        public static int GetUserIDFromClaims(ControllerBase controller)
        {
            var user = controller.HttpContext.User;

            //if (!user.Identity.IsAuthenticated)
            //{
            //    return 0;
            //}

            var userIdClaim = user?.Claims?.FirstOrDefault(c => c.Type == "Uid");
            if (userIdClaim != null)
            {
                return Convert.ToInt32(userIdClaim.Value);
            }

            return 0;
        }
    }
}
