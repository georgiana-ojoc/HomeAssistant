using MediatR;

namespace API.Queries.Room
{
    public class RoomByIdQuery : IRequest<Shared.Models.Room>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int Id { get; set; }

        public RoomByIdQuery(string email, int houseId, int id)
        {
            Email = email;
            HouseId = houseId;
            Id = id;
        }
    }
}