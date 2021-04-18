using System;
using MediatR;

namespace API.Queries.Thermostat
{
    public class ThermostatByIdQuery : IRequest<Shared.Models.Thermostat>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public ThermostatByIdQuery(string email, Guid houseId, Guid roomId, Guid id)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}