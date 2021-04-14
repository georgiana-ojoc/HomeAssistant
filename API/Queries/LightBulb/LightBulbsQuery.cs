using System.Collections.Generic;
using MediatR;

namespace API.Queries.LightBulb
{
    public class LightBulbsQuery: IRequest<IEnumerable<Shared.Models.LightBulb>>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public LightBulbsQuery(int userId, int houseId, int roomId)
        {
            UserId = userId;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}