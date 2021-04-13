using System.Collections.Generic;
using MediatR;

namespace API.Queries.Room
{
    public class RoomsQuery : IRequest<IEnumerable<Models.Room>>
    {
        public int UserId { get; set; }
        public int HouseId { get; set; }

        public RoomsQuery(int userId, int houseId)
        {
            UserId = userId;
            HouseId = houseId;
        }
    }
}