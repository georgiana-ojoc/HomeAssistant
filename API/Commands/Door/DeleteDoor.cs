using MediatR;

namespace API.Commands.Door
{
    public class DeleteDoor : IRequest<Shared.Models.Door>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public int Id { get; set; }

        public DeleteDoor(int userId, int houseId, int roomId, int id)
        {
            UserId = userId;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}