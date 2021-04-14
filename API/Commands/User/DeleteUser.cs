using MediatR;

namespace API.Commands.User
{
    public class DeleteUser : IRequest<Shared.Models.User>
    {
        public int Id { get; set; }

        public DeleteUser(int id)
        {
            Id = id;
        }
    }
}