using MediatR;

namespace API.Queries.House
{
    public class HouseById : IRequest<Shared.Models.House>
    {
        public int UserId { get; set; }
        public int Id { get; set; }

        public HouseById(int userId, int id)
        {
            UserId = userId;
            Id = id;
        }
    }
}