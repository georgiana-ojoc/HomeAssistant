using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.LightBulbCommand
{
    public class GetLightBulbCommandsQuery : IRequest<IEnumerable<Shared.Models.LightBulbCommand>>
    {
        public Guid ScheduleId { get; set; }
    }
}