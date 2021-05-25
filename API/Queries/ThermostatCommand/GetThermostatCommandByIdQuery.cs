using System;
using MediatR;

namespace API.Queries.ThermostatCommand
{
    public class GetThermostatCommandByIdQuery : IRequest<Models.ThermostatCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}