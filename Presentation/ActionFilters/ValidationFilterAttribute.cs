using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class ValidationFilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];

            var param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;

            if(param is null)
            {
                context.Result = new BadRequestObjectResult($"Objekt null geldi" + $"controller : {controller} " + $"action {action}");
                return;//400 Döndük
            }

            if (context.ModelState.IsValid)
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);//422 döndük
        }
    }
}
