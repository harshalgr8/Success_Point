using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SucessPointCore.Api.Filters
{
    public class SwaggerIgnoreFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var allowAnonymous = context.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType() == typeof(AllowAnonymousAttribute));

            if (allowAnonymous)
            {
                operation.Security = null;
            }
        }
    }

}
