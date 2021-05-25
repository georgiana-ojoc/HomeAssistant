using System;
using MediatR;

namespace HomeAssistantAPI.Commands.ThermostatCommand
{
    public class DeleteThermostatCommandCommand : IRequest<Models.ThermostatCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}