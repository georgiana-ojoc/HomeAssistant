using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.User.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUser, Shared.Models.User>
    {
        private readonly IUserRepository _repository;

        public AddUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.User> Handle(AddUser request, CancellationToken cancellationToken)
        {
            var result = await _repository.CreateUserAsync(request.User);
            return result;
        }
    }
}