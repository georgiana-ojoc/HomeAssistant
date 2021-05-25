using System;
using System.Collections.Generic;
using API.Responses;
using MediatR;

namespace API.Queries.LightBulbCommand
{
    public class GetLightBulbCommandsQuery : IRequest<IEnumerable<LightBulbCommandResponse>>
    {
        public Guid ScheduleId { get; set; }
    }
}