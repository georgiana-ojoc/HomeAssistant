using System;
using MediatR;

namespace API.Commands.Thermostat
{
    public class AddThermostatCommand : IRequest<Shared.Models.Thermostat>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public Shared.Models.Thermostat Thermostat { get; set; }

        public AddThermostatCommand(string email, Guid houseId, Guid roomId, Shared.Models.Thermostat thermostat)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Thermostat = thermostat;
        }
    }
}