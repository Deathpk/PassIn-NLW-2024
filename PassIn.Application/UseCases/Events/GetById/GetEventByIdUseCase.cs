using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetById;

public class GetEventByIdUseCase
{
    public ResponseEventJson Execute(Guid id)
    {
        var dbContext = new PassInDbContext();

        var eventEntity = dbContext.Events
            .Include(ev => ev.Attendees)
            .FirstOrDefault(ev => ev.Id == id);
        
        if (eventEntity is null)
            throw new NotFoundException("An event with this Id Was not found.");

        return new ResponseEventJson()
        {
            Id = eventEntity.Id,
            Title = eventEntity.Title,
            Details = eventEntity.Details,
            MaximumAttendees = eventEntity.Maximum_Attendees,
            AttendeesAmount = eventEntity.Attendees.Count(),
        };
    }
}