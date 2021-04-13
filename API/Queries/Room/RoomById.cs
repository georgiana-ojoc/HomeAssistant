using MediatR;

namespace API.Queries.Room
{
    public class RoomById: IRequest<Models.Room>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int Id { get; set; }

        public RoomById(int userId, int houseId, int id)
        {
            UserId = userId;
            HouseId = houseId;
            Id = id;
        }
    }
}