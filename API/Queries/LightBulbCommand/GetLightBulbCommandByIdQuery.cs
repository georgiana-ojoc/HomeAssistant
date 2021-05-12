using System;
using MediatR;

namespace API.Queries.LightBulbCommand
{
    public class GetLightBulbCommandByIdQuery:IRequest<Shared.Models.LightBulbCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
        
    }
}