using System;
using API.Responses;
using MediatR;

namespace API.Queries.ThermostatCommand
{
    public class GetThermostatCommandByIdQuery : IRequest<ThermostatCommandResponse>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}