using System;
using API.Requests;
using MediatR;

namespace API.Commands.ThermostatCommand
{
    public class CreateThermostatCommandCommand : IRequest<Models.ThermostatCommand>
    {
        public Guid ScheduleId { get; set; }

        public ThermostatCommandRequest Request { get; set; }
    }
}