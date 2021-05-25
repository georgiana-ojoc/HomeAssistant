using System;
using MediatR;

namespace API.Commands.Thermostat
{
    public class DeleteThermostatCommand : IRequest<Models.Thermostat>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
    }
}