using System;
using MediatR;
using Shared.Requests;

namespace API.Commands.Thermostat
{
    public class CreateThermostatCommand : IRequest<Shared.Models.Thermostat>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public ThermostatRequest Request { get; set; }
    }
}