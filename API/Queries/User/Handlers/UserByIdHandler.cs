using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using MediatR;

namespace API.Queries.Handlers
{
    public class UserByIdHandler : IRequestHandler<UserById, User>
    {
        private readonly IUserRepository _repository;

        public UserByIdHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> Handle(UserById request, CancellationToken cancellationToken)
        {
            User user = await _repository.GetUserByIdAsync(request.Id);
            return user;
        }
    }
}