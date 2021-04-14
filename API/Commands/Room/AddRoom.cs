using MediatR;

namespace API.Commands.Room
{
    public class AddRoom : IRequest<Shared.Models.Room>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public Shared.Models.Room Room { get; set; }

        public AddRoom(int userId, int houseId, Shared.Models.Room room)
        {
            UserId = userId;
            HouseId = houseId;
            Room = room;
        }
    }
}