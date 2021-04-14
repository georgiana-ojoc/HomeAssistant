using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.User.Handlers
{
    public class UserByIdHandler : IRequestHandler<UserById, Shared.Models.User>
    {
        private readonly IUserRepository _repository;

        public UserByIdHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.User> Handle(UserById request, CancellationToken cancellationToken)
        {
            Shared.Models.User user = await _repository.GetUserByIdAsync(request.Id);
            return user;
        }
    }
}