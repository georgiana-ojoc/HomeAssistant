using System.Collections.Generic;
using MediatR;

namespace API.Queries.Room
{
    public class RoomsQuery : IRequest<IEnumerable<Shared.Models.Room>>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }

        public RoomsQuery(string email, int houseId)
        {
            Email = email;
            HouseId = houseId;
        }
    }
}