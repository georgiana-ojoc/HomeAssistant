using MediatR;

namespace API.Commands.Room
{
    public class DeleteRoom: IRequest<Models.Room>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public int Id { get; set; }

        public DeleteRoom(int userId, int houseId, int id)
        {
            UserId = userId;
            HouseId = houseId;
            Id = id;
        }
    }
}