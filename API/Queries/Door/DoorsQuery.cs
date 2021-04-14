using System.Collections.Generic;
using MediatR;

namespace API.Queries.Door
{
    public class DoorsQuery : IRequest<IEnumerable<Shared.Models.Door>>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public DoorsQuery(string email, int houseId, int roomId)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}