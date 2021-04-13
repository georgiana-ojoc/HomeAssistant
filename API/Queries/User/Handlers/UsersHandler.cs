using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using MediatR;

namespace API.Queries.Handlers
{
    public class UsersQueryHandler: IRequestHandler<UsersQuery,IEnumerable<User>>
    {
        private readonly IUserRepository _repository;
        public UsersQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<User>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<User> users = await  _repository.GetUsersAsync();
            return users;
        }
    }
}