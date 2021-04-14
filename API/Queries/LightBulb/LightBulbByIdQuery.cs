using MediatR;

namespace API.Queries.LightBulb
{
    public class LightBulbByIdQuery : IRequest<Shared.Models.LightBulb>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public int Id { get; set; }

        public LightBulbByIdQuery(string email, int houseId, int roomId, int id)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}