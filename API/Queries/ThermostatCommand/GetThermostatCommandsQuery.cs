using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.ThermostatCommand
{
    public class GetThermostatCommandsQuery : IRequest<IEnumerable<Shared.Models.ThermostatCommand>>
    {
        public Guid ScheduleId { get; set; }
    }
}