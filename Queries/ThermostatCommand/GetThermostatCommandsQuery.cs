using System;
using System.Collections.Generic;
using HomeAssistantAPI.Responses;
using MediatR;

namespace HomeAssistantAPI.Queries.ThermostatCommand
{
    public class GetThermostatCommandsQuery : IRequest<IEnumerable<ThermostatCommandResponse>>
    {
        public Guid ScheduleId { get; set; }
    }
}