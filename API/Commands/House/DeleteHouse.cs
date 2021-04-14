using MediatR;

namespace API.Commands.House
{
    public class DeleteHouse : IRequest<Shared.Models.House>
    {
        public int UserId { get; set; }
        public int Id { get; set; }

        public DeleteHouse(int userId, int id)
        {
            UserId = userId;
            Id = id;
        }
    }
}