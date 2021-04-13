using MediatR;

namespace API.Commands.Room
{
    public class AddRoom: IRequest<Models.Room>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public Models.Room Room { get; set; }

        public AddRoom(int userId, int houseId, Models.Room room)
        {
            UserId = userId;
            HouseId = houseId;
            Room = room;
        }
    }
}