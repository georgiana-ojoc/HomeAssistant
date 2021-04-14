using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.User.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUser, Shared.Models.User>
    {
        private readonly IUserRepository _repository;

        public DeleteUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.User> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteUserAsync(request.Id);
            return result;
        }
    }
}