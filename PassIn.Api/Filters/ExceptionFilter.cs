using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PassIn.Communication.Responses;
using PassIn.Exceptions;

namespace PassIn.Api.Filters;

public class ExceptionFilter: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var result = context.Exception is PassInException;

        if (result)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnknownError(context);
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {
        if (context.Exception is NotFoundException)
        {
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
            context.Result = new NotFoundObjectResult(new ResponseErrorJson(context.Exception.Message));
        }
        
        if (context.Exception is ErrorOnValidationException)
        {
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            context.Result = new BadRequestObjectResult(new ResponseErrorJson(context.Exception.Message));
        }
        
        if (context.Exception is ConflictException)
        {
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Conflict;
            context.Result = new ConflictObjectResult(new ResponseErrorJson(context.Exception.Message));
        }
        
        if (context.Exception is NotAcceptableException)
        {
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotAcceptable;
            context.Result = new ObjectResult(new ResponseErrorJson(context.Exception.Message));
        }
    }
    
    private void ThrowUnknownError(ExceptionContext context)
    {
        //Setando o Status code do contexto HTTP.
        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        //Instanciando o resultado do Contexto/Requisicao.
        context.Result = new ObjectResult(new ResponseErrorJson("Unknow error"));
    }
}