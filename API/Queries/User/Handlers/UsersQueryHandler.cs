using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.User.Handlers
{
    public class UsersQueryHandler : IRequestHandler<UsersQuery, IEnumerable<Shared.Models.User>>
    {
        private readonly IUserRepository _repository;

        public UsersQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Shared.Models.User>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Shared.Models.User> users = await _repository.GetUsersAsync();
            return users;
        }
    }
}