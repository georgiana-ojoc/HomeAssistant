using MediatR;

namespace API.Commands.LightBulb
{
    public class DeleteLightBulbCommand : IRequest<Shared.Models.LightBulb>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public int Id { get; set; }

        public DeleteLightBulbCommand(string email, int houseId, int roomId, int id)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}