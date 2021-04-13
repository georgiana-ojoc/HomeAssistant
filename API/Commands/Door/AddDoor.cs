using API.Models;
using MediatR;

namespace API.Commands
{
    public class AddDoor : IRequest<Door>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public Door Door { get; set; }

        public AddDoor(int userId, int houseId, int roomId, Door door)
        {
            UserId = userId;
            HouseId = houseId;
            RoomId = roomId;
            Door = door;
        }
    }
}