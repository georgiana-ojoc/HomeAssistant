using System;
using MediatR;
using Shared.Requests;

namespace API.Commands.ThermostatCommand
{
    public class CreateThermostatCommandCommand : IRequest<Shared.Models.ThermostatCommand>
    {
        public Guid ScheduleId { get; set; }

        public ThermostatCommandRequest Request { get; set; }
    }
}