using API.Models;
using MediatR;

namespace API.Queries
{
    public class DoorById : IRequest<Door>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public int Id { get; set; }

        public DoorById(int userId, int houseId, int roomId, int id)
        {
            UserId = userId;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}