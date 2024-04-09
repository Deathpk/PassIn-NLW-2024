using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.CheckIn.RegisterCheckIn;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;

namespace PassIn.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckInController: ControllerBase
{
    [HttpPost]
    [Route("{attendeeId}")]
    [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public IActionResult Register([FromRoute] Guid attendeeId)
    {
        var useCase = new RegisterCheckInUseCase(attendeeId);
        var response = useCase.Execute();
        return Created(string.Empty, response);
    }
}