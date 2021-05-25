using System;
using MediatR;

namespace HomeAssistantAPI.Queries.LightBulbCommand
{
    public class GetLightBulbCommandByIdQuery : IRequest<Models.LightBulbCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}