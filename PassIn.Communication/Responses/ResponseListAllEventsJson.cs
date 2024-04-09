using PassIn.Infrastructure.Entities;

namespace PassIn.Communication.Responses;

public class ResponseListAllEventsJson
{
    public IEnumerable<Event> Events { get; set; }
}