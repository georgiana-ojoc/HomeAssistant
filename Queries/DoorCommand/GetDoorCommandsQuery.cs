using System;
using System.Collections.Generic;
using HomeAssistantAPI.Responses;
using MediatR;

namespace HomeAssistantAPI.Queries.DoorCommand
{
    public class GetDoorCommandsQuery : IRequest<IEnumerable<DoorCommandResponse>>
    {
        public Guid ScheduleId { get; set; }
    }
}