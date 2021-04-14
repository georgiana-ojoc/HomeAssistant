using System.Collections.Generic;
using MediatR;

namespace API.Queries.Thermostat
{
    public class ThermostatsQuery : IRequest<IEnumerable<Shared.Models.Thermostat>>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public ThermostatsQuery(string email, int houseId, int roomId)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}