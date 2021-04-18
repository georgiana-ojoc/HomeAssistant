using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.Thermostat
{
    public class ThermostatsQuery : IRequest<IEnumerable<Shared.Models.Thermostat>>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public ThermostatsQuery(string email, Guid houseId, Guid roomId)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}