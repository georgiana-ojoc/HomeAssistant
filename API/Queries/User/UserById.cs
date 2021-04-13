using API.Models;
using MediatR;

namespace API.Queries
{
    public class UserById : IRequest<User>
    {
        public int Id { get; set; }

        public UserById(int id)
        {
            Id = id;
        }
    }
}