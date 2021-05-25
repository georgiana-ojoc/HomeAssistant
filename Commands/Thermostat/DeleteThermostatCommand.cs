using System;
using MediatR;

namespace HomeAssistantAPI.Commands.Thermostat
{
    public class DeleteThermostatCommand : IRequest<Models.Thermostat>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
    }
}