using MediatR;

namespace API.Commands.Room
{
    public class DeleteRoomCommand : IRequest<Shared.Models.Room>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int Id { get; set; }

        public DeleteRoomCommand(string email, int houseId, int id)
        {
            Email = email;
            HouseId = houseId;
            Id = id;
        }
    }
}