using System;
using System.Collections.Generic;
using MediatR;
using Shared.Responses;

namespace API.Queries.ThermostatCommand
{
    public class GetThermostatCommandsQuery : IRequest<IEnumerable<ThermostatCommandResponse>>
    {
        public Guid ScheduleId { get; set; }
    }
}