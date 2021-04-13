using System.Collections.Generic;
using API.Models;
using MediatR;

namespace API.Queries
{
    public class Doors: IRequest<IEnumerable<Door>>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public Doors(int userId, int houseId, int roomId)
        {
            UserId = userId;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}