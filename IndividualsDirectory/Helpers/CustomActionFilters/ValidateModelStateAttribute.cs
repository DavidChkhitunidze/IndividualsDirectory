using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Helpers.CustomActionFilters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var controller = context.Controller as Controller;
                var model = context.ActionArguments.Any() ? context.ActionArguments.First().Value : null;

                context.Result = (IActionResult)controller?.View(model) ?? new BadRequestResult();
            }

            base.OnActionExecuting(context);
        }
    }
}
