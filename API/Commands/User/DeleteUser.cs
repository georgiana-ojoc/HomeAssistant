using API.Models;
using MediatR;

namespace API.Commands
{
    public class DeleteUser : IRequest<User>
    {
        public int Id { get; set; }

        public DeleteUser(int id)
        {
            Id = id;
        }
    }
}