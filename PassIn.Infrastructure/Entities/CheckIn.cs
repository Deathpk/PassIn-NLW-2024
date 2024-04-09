using System.ComponentModel.DataAnnotations.Schema;

namespace PassIn.Infrastructure.Entities;

public class CheckIn
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Created_At { get; set; }
    //Como visto na aula 3, a relação foi criada de forma errada, pois a referencia
    //Deveria estar no Attendee, já que se trata de uma relação 1:1, onde
    //1 Attendee possui 1 CheckIn.
    [ForeignKey("Attendee_Id")] public Attendee Attendee { get; set; } = default!;
}