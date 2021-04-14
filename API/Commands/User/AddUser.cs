using MediatR;

namespace API.Commands.User
{
    public class AddUser : IRequest<Shared.Models.User>
    {
        public Shared.Models.User User { get; set; }

        public AddUser(Shared.Models.User user)
        {
            User = user;
        }
    }
}