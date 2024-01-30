using Utilities;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace PayrollManagementSystem.Business
{
    public static class HelperExtension
    {
        public static NotFoundObjectResult NotFound<T>(this T obj, string message)
        {
            return new NotFoundObjectResult(Response<T>.NotFound(message));
        }

        public static NotFoundObjectResult NotFound<T>(this T obj)
        {
            return new NotFoundObjectResult(Response<T>.NotFound(obj?.ToString()));
        }

        public static BadRequestObjectResult BadRequest<T>(this T obj, string message)
        {
            return new BadRequestObjectResult(Response<T>.BadRequest(message));
        }

        public static BadRequestObjectResult BadRequest<T>(this T obj)
        {
            if (typeof(T) == typeof(Exception))
            {
                var ex = obj as Exception;
                while (ex?.InnerException != null) ex = ex.InnerException;
                return new BadRequestObjectResult(ErrorResponse.BadRequest(ex?.Message));
            }

            return new BadRequestObjectResult(ErrorResponse.BadRequest(obj?.ToString()));
        }

        public static OkObjectResult Ok<T>(this T obj, string message = "Executed Successfully!")
        {
            return new OkObjectResult(SuccessResponse<T>.Ok(message, obj));
        }
        public static CreatedResult Created<T>(this T obj, string pathWithId)
        {
            return new CreatedResult(new Uri(pathWithId, UriKind.Relative), obj);
        }
        public static OkObjectResult Ok<T, TT>(this T obj, string message = "Executed Successfully!", TT? miscD = default)
        {
            return new OkObjectResult(SuccessAddResponse<T, TT>.Ok(message, obj, miscD));
        }

        public static IActionResult HandleError(this Exception e)
        {
            if (e is EntityNotFoundException)
            {
                return e.GetBaseException().Message.NotFound();
            }

            return e.GetBaseException().Message.BadRequest();
        }
    }
}
