using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using MediatR;

namespace API.Commands.Handlers
{
    public class AddUserHandler: IRequestHandler<AddUser,User>
    {
        private readonly IUserRepository _repository;
        public AddUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<User> Handle(AddUser request, CancellationToken cancellationToken)
        {
            var result = await _repository.CreateUser(request.User);
            return result;
        }
    }
}