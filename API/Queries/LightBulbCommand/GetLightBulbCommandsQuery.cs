using System;
using System.Collections.Generic;
using MediatR;
using Shared.Responses;

namespace API.Queries.LightBulbCommand
{
    public class GetLightBulbCommandsQuery : IRequest<IEnumerable<LightBulbCommandResponse>>
    {
        public Guid ScheduleId { get; set; }
    }
}