using System.Collections.Generic;
using MediatR;

namespace API.Queries.LightBulb
{
    public class LightBulbsQuery : IRequest<IEnumerable<Shared.Models.LightBulb>>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public LightBulbsQuery(string email, int houseId, int roomId)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}