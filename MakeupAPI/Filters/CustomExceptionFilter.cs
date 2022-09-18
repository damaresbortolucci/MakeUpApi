using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MakeupAPI.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NullReferenceException nullReference)
            {
                context.Result = new ObjectResult(new
                {
                    message = "ID não encontrado."
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            else if (context.Exception is KeyNotFoundException keyNotfount)
            {
                context.Result = new ObjectResult(new
                {
                    message = "ID não encontrado."
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            else
            {
                context.Result = new ObjectResult(new
                {
                    message = "Ocorreu um erro inesperado. Tente mais tarde."
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            context.ExceptionHandled = true;
        }
    }
}
