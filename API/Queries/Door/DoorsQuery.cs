using System.Collections.Generic;
using MediatR;

namespace API.Queries.Door
{
    public class DoorsQuery : IRequest<IEnumerable<Shared.Models.Door>>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public DoorsQuery(int userId, int houseId, int roomId)
        {
            UserId = userId;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}