using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Attendees.GetAllByEventId;

public class ListAttendeesUseCase
{
    private readonly Guid _eventId;
    private readonly PassInDbContext _dbContext = new PassInDbContext();

    public ListAttendeesUseCase(Guid eventId)
    {
        _eventId = eventId;
    }

    public ResponseAllAttendeesjson Execute()
    {
        var entity = _dbContext.Events
            .Include(eventEntity => eventEntity.Attendees)
            .ThenInclude(attendee => attendee.CheckIn)
            .FirstOrDefault(eventEntity => eventEntity.Id == _eventId);

        if (entity is null)
            throw new NotFoundException("An event with the provided Id was not found on the database.");
        
        return new ResponseAllAttendeesjson
        {
            Attendees = entity.Attendees.Select(attendee => new ResponseAttendeeJson
            {
                Id = attendee.Id,
                Email = attendee.Email,
                Name = attendee.Name,
                CreatedAt = attendee.Created_At,
                CheckedInAt = attendee.CheckIn?.Created_At
            }).ToList()
        };
    }
}