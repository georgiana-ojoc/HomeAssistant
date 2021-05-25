using System;
using System.Collections.Generic;
using HomeAssistantAPI.Responses;
using MediatR;

namespace HomeAssistantAPI.Queries.LightBulbCommand
{
    public class GetLightBulbCommandsQuery : IRequest<IEnumerable<LightBulbCommandResponse>>
    {
        public Guid ScheduleId { get; set; }
    }
}