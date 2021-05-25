using System;
using HomeAssistantAPI.Requests;
using MediatR;

namespace HomeAssistantAPI.Commands.Thermostat
{
    public class CreateThermostatCommand : IRequest<Models.Thermostat>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public ThermostatRequest Request { get; set; }
    }
}