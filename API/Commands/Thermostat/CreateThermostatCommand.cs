using System;
using API.Requests;
using MediatR;

namespace API.Commands.Thermostat
{
    public class CreateThermostatCommand : IRequest<Models.Thermostat>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public ThermostatRequest Request { get; set; }
    }
}