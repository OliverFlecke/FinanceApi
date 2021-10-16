using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinanceApi;

class HttpResponseExceptionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        switch (context.Exception)
        {
            case EntityNotFoundException ex:
                context.Result = new NotFoundObjectResult(ex.Message);
                context.ExceptionHandled = true;
                break;
        }
    }
}