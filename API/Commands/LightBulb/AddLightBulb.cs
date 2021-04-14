using MediatR;

namespace API.Commands.LightBulb
{
    public class AddLightBulb: IRequest<Shared.Models.LightBulb>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public Shared.Models.LightBulb LightBulb;

        public AddLightBulb( int userId, int houseId, int roomId,Shared.Models.LightBulb lightBulb)
        {
            LightBulb = lightBulb;
            UserId = userId;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}