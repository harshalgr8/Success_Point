using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace SuccessPointCore.Application.Custom_Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthUserTypeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly int[] _allowedUserTypes;

        public AuthUserTypeAttribute(params int[] allowedUserTypes)
        {
            _allowedUserTypes = allowedUserTypes;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var utypeClaim = user.Claims.FirstOrDefault(c => c.Type == "utype");

            if (utypeClaim == null || !int.TryParse(utypeClaim.Value, out int utype) || !_allowedUserTypes.Contains(utype))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
