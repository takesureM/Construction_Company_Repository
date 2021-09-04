using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationService.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthorizationService.SwaggerFilters
{
    public class ReqiuredRolesDescriptionFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
          
            var attr = context.MethodInfo.GetCustomAttributes(true).FirstOrDefault(c => c is AuthorizeEnumAttribute);

            if (attr == null)
            {
                operation.Description = "Required roles: [Anonymous]";
                return;
            }

            var rolesAttrttr = (AuthorizeEnumAttribute)attr;
            var roles = rolesAttrttr.Roles.Split(',');

            var descr = roles.Aggregate("", (current, t) => current + $"[{t}] ");

            operation.Description = $"Required roles: {descr}";

        }
    }
}
