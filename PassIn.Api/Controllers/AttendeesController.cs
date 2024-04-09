using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Attendees.GetAllByEventId;
using PassIn.Application.UseCases.Events.RegisterAttendee;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;

namespace PassIn.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AttendeesController: ControllerBase
{
    [HttpPost]
    [Route("/event/{id}/register")]
    [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status406NotAcceptable)]
    public IActionResult Register([FromRoute] Guid id, [FromBody] RequestRegisterEventJson request)
    {
        var useCase = new RegisterAttendeeOnEventUseCase(id, request);
        var response = useCase.Execute();
        return Created(string.Empty, response);
    }

    [HttpGet]
    [Route("/event/{id}")]
    [ProducesResponseType(typeof(ResponseAllAttendeesjson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public IActionResult ListEventAttendees([FromRoute] Guid id)
    {
        var useCase = new ListAttendeesUseCase(id);
        var response = useCase.Execute();
        return Ok(response);
    }
}