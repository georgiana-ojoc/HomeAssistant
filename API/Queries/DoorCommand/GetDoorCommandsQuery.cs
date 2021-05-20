using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.DoorCommand
{
    public class GetDoorCommandsQuery : IRequest<IEnumerable<Shared.Responses.DoorCommandResponse>>
    {
        public Guid ScheduleId { get; set; }
    }
}