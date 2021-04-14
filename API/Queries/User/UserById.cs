using MediatR;

namespace API.Queries.User
{
    public class UserById : IRequest<Shared.Models.User>
    {
        public int Id { get; set; }

        public UserById(int id)
        {
            Id = id;
        }
    }
}