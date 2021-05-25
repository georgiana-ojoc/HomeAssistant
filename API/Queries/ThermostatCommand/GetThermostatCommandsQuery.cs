using System;
using System.Collections.Generic;
using API.Responses;
using MediatR;

namespace API.Queries.ThermostatCommand
{
    public class GetThermostatCommandsQuery : IRequest<IEnumerable<ThermostatCommandResponse>>
    {
        public Guid ScheduleId { get; set; }
    }
}