using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TheatreProject.PerformanceAPI.Models.DTOs;

namespace TheatreProject.PerformanceAPI.Validators;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errorsInModelState = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

            var responseDto = new ResponseDto
            {
                IsSuccess = false,
                ErrorMessages = errorsInModelState.SelectMany(x => x.Value).ToList()
            };

            context.Result = new BadRequestObjectResult(responseDto);
            return;
        }

        await next();
    }
}