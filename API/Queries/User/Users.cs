using System.Collections.Generic;
using API.Models;
using MediatR;

namespace API.Queries
{
    public class UsersQuery: IRequest<IEnumerable<User>>
    {
        public UsersQuery()
        {

        }
    }
}