using MediatR;

namespace API.Commands.LightBulb
{
    public class AddLightBulb: IRequest<Models.LightBulb>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public Models.LightBulb LightBulb;

        public AddLightBulb( int userId, int houseId, int roomId,Models.LightBulb lightBulb)
        {
            LightBulb = lightBulb;
            UserId = userId;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}