using API.Models;
using MediatR;

namespace API.Queries
{
    public class UserByIdQuery:  IRequest<User>
    {
        public int Id { get; set; }

        public UserByIdQuery(int id)
        {
            Id = id;
        }
    }
}