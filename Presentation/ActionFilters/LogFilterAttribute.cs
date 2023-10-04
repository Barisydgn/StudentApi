using Entities.LogModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class LogFilterAttribute:ActionFilterAttribute
    {
        private readonly ILogService _logger;

        public LogFilterAttribute(ILogService logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInfo(Log("OnActionExecuting", context.RouteData));
        }

        private string Log(string ModelName, RouteData routeData)
        {
            var logDetails = new LogDetails()
            {
                Action = routeData.Values["action"],
                Controller = routeData.Values["controller"],
                ModelName = ModelName
            };
            if(routeData.Values.Count()>3)
                logDetails.Id= routeData.Values["Id"];
           return logDetails.ToString();
        }
    }
}
