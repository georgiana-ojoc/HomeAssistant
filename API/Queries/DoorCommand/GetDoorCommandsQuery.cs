using System;
using System.Collections.Generic;
using API.Responses;
using MediatR;

namespace API.Queries.DoorCommand
{
    public class GetDoorCommandsQuery : IRequest<IEnumerable<DoorCommandResponse>>
    {
        public Guid ScheduleId { get; set; }
    }
}