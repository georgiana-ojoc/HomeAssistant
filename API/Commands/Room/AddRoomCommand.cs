using MediatR;

namespace API.Commands.Room
{
    public class AddRoomCommand : IRequest<Shared.Models.Room>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public Shared.Models.Room Room { get; set; }

        public AddRoomCommand(string email, int houseId, Shared.Models.Room room)
        {
            Email = email;
            HouseId = houseId;
            Room = room;
        }
    }
}