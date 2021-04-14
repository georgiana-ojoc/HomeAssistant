using MediatR;

namespace API.Queries.LightBulb
{
    public class LightBulbById: IRequest<Models.LightBulb>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public int Id { get; set; }

        public LightBulbById(int userId, int houseId, int roomId, int id)
        {
            UserId = userId;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}