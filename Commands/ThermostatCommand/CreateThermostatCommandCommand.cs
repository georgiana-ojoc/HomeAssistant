using System;
using HomeAssistantAPI.Requests;
using MediatR;

namespace HomeAssistantAPI.Commands.ThermostatCommand
{
    public class CreateThermostatCommandCommand : IRequest<Models.ThermostatCommand>
    {
        public Guid ScheduleId { get; set; }

        public ThermostatCommandRequest Request { get; set; }
    }
}