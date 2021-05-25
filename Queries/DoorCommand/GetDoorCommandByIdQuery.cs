using System;
using MediatR;

namespace HomeAssistantAPI.Queries.DoorCommand
{
    public class GetDoorCommandByIdQuery : IRequest<Models.DoorCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}