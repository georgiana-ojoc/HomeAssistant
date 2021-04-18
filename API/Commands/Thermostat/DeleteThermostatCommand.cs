using System;
using MediatR;

namespace API.Commands.Thermostat
{
    public class DeleteThermostatCommand : IRequest<Shared.Models.Thermostat>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public DeleteThermostatCommand(string email, Guid houseId, Guid roomId, Guid id)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}