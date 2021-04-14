using MediatR;

namespace API.Commands.Door
{
    public class AddDoor : IRequest<Shared.Models.Door>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public Shared.Models.Door Door { get; set; }

        public AddDoor(int userId, int houseId, int roomId, Shared.Models.Door door)
        {
            UserId = userId;
            HouseId = houseId;
            RoomId = roomId;
            Door = door;
        }
    }
}