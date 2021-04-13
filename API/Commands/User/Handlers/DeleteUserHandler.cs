using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using MediatR;

namespace API.Commands.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUser, User>
    {
        private readonly IUserRepository _repository;

        public DeleteUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteUser(request.Id);
            return result;
        }
    }
}