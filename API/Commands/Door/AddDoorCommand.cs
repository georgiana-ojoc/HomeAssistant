using MediatR;

namespace API.Commands.Door
{
    public class AddDoorCommand : IRequest<Shared.Models.Door>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public Shared.Models.Door Door { get; set; }

        public AddDoorCommand(string email, int houseId, int roomId, Shared.Models.Door door)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Door = door;
        }
    }
}