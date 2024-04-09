using PassIn.Communication.Responses;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.ListAll;

public class ListAllEventsUseCase
{
    private readonly PassInDbContext _dbContext;
    
    public ListAllEventsUseCase()
    {
        _dbContext = new PassInDbContext();
    }

    public ResponseListAllEventsJson Execute()
    {
        var eventList = _dbContext.Events.ToList();
        return new ResponseListAllEventsJson
        {
            Events = eventList
        };
    }
}