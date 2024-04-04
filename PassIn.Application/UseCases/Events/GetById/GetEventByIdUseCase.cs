using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetById;

public class GetEventByIdUseCase
{
    public ResponseEventJson Execute(Guid id)
    {
        var dbContext = new PassInDbContext();
        
        var eventEntity = dbContext.Events.Find(id);
        if (eventEntity is null)
            throw new NotFoundException("An event with this Id Was not found.");

        return new ResponseEventJson()
        {
            Id = eventEntity.Id,
            Title = eventEntity.Title,
            Details = eventEntity.Details,
            MaximumAttendees = eventEntity.Maximum_Attendees,
            AttendeesAmount = -1
        };
    }
}