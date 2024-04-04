using System.Net.Mail;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Events.RegisterAttendee;

public class RegisterAttendeeOnEventUseCase
{
    private readonly PassInDbContext _dbContext;
    private readonly Guid _eventId;
    private readonly string _email;
    private readonly string _name;
    
    public RegisterAttendeeOnEventUseCase(Guid eventId, RequestRegisterEventJson request)
    {
        _dbContext = new PassInDbContext();
        _eventId = eventId;
        _email = request.Email;
        _name = request.Name;
    }
    
    public ResponseRegisteredJson Execute()
    {
        Validate();
        var entity = new Attendee()
        {
            Event_Id = _eventId,
            Email = _email,
            Name = _name,
            //UtcNow pega a data e hora Utc, e somente o Now pega a data e hora da maquina.
            Created_At = DateTime.UtcNow
        };

        _dbContext.Attendees.Add(entity);
        _dbContext.SaveChanges();

        return new ResponseRegisteredJson { Id = entity.Id };
    }

    private void Validate()
    {
        var eventEntity = _dbContext.Events.Find(_eventId);
        if (eventEntity is null)
            throw new NotFoundException("An event with this Id does not exist.");
        
        if (EventMaximumCapacityAchieved(eventEntity.Maximum_Attendees))
            throw new ErrorOnValidationException("The Event has reached It's maximum capacity.");

        if (string.IsNullOrWhiteSpace(_email))
            throw new ErrorOnValidationException("The email field is Invalid.");
        
        if (!EmailIsValid(_email))
            throw new ErrorOnValidationException("The entered Email is not a valid Mail Address.");
        
        if (string.IsNullOrWhiteSpace(_name))
            throw new ErrorOnValidationException("The name field is Invalid.");
        
        if (AttendeeAlreadyRegisteredOnEvent())
            throw new ConflictException("You can not register twice on the same event");
    }

    private static bool EmailIsValid(string email)
    {
        try
        {
            //Se o e-mail for invalido, ira estourar uma excecao.
            new MailAddress(email);
            return true;
        } catch
        {
            return false;
        }
    }

    private bool EventMaximumCapacityAchieved(int maximumAttendees)
    {
        var attendeesQuantity = _dbContext.Attendees
            .Count(attendees => attendees.Event_Id == _eventId);

        return attendeesQuantity == maximumAttendees;
    }

    private bool AttendeeAlreadyRegisteredOnEvent()
    {
        return _dbContext
            .Attendees
            .Any(attendee => attendee.Event_Id == _eventId && attendee.Email.Equals(_email));
    }
}