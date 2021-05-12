using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.DoorCommand
{
    public class GetDoorCommandsQuery:IRequest<IEnumerable<Shared.Models.DoorCommand>>
    {
        public Guid ScheduleId { get; set; }
    }
}