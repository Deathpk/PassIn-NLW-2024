using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.CheckIn.RegisterCheckIn;

public class RegisterCheckInUseCase
{
    private readonly PassInDbContext _dbContext = new PassInDbContext();
    private readonly Guid _attendeeId;
    private Attendee? _attendee;
    
    public RegisterCheckInUseCase(Guid attendeeeId)
    {
        _attendeeId = attendeeeId;
    }

    public ResponseRegisteredJson Execute()
    {
        _attendee = _dbContext.Attendees.Find(_attendeeId);
        
        Validate();
        
        var checkIn = _dbContext.CheckIns.Add(new Infrastructure.Entities.CheckIn
        {
            Attendee = _attendee,
            Created_At = DateTime.UtcNow
        });

        _dbContext.SaveChanges();

        return new ResponseRegisteredJson
        {
            Id = checkIn.Entity.Id
        };
    }

    private void Validate()
    {
        if (_attendee is null)
            throw new NotFoundException("An Attendee with this Id was not found on the Database");

        var attendeeAlreadyCheckedIn = _dbContext.CheckIns.Any(checkIn => checkIn.Attendee.Id == _attendeeId);
        
        if (attendeeAlreadyCheckedIn)
            throw new ConflictException("You cannot checkIn twice at the same Event.");
    }
}