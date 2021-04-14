using System.Collections.Generic;
using MediatR;

namespace API.Queries.User
{
    public class UsersQuery : IRequest<IEnumerable<Shared.Models.User>>
    {
        public UsersQuery()
        {
        }
    }
}