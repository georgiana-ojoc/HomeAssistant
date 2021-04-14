using MediatR;

namespace API.Queries.House
{
    public class HouseByIdQuery : IRequest<Shared.Models.House>
    {
        public string Email { get; set; }
        public int Id { get; set; }

        public HouseByIdQuery(string email, int id)
        {
            Email = email;
            Id = id;
        }
    }
}