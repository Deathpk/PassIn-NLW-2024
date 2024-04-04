using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Events.Register;

public class RegisterEventUseCase
{
    public async Task<ResponseRegisteredJson> Execute(RequestEventJson request)
    {
        Validate(request);
        var dbContext = new PassInDbContext();
        var entity = new Event
        {
            Title = request.Title,
            Details = request.Details,
            Maximum_Attendees = request.MaximumAttendees,
            Slug = request.Title.ToLower().Replace(" ", "-"),
        };
        await dbContext.Events.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return new ResponseRegisteredJson
        {
            Id = entity.Id
        };
    }

    private void Validate(RequestEventJson request)
    {
        if(request.MaximumAttendees <= 0)
        {
            throw new ErrorOnValidationException("The Maximum Attendees field is invalid.");
        }

        if(string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ErrorOnValidationException("The Title field is invalid.");
        }
        
        if(string.IsNullOrWhiteSpace(request.Details))
        {
            throw new ErrorOnValidationException("The Details field is invalid.");
        }
    }
}