using MediatR;

namespace API.Commands.LightBulb
{
    public class AddLightBulbCommand : IRequest<Shared.Models.LightBulb>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public Shared.Models.LightBulb LightBulb { get; set; }

        public AddLightBulbCommand(string email, int houseId, int roomId, Shared.Models.LightBulb lightBulb)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            LightBulb = lightBulb;
        }
    }
}