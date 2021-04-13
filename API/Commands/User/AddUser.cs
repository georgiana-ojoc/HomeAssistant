using API.Models;
using MediatR;

namespace API.Commands
{
    public class AddUser: IRequest<User>
    {
        public User User { get; set; }

        public AddUser(User user)
        {
            User = user;
        }
    }
}