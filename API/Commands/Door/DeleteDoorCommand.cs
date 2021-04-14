using MediatR;

namespace API.Commands.Door
{
    public class DeleteDoorCommand : IRequest<Shared.Models.Door>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public int Id { get; set; }

        public DeleteDoorCommand(string email, int houseId, int roomId, int id)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}