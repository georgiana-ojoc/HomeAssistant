using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.Door
{
    public class DoorsQuery : IRequest<IEnumerable<Shared.Models.Door>>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public DoorsQuery(string email, Guid houseId, Guid roomId)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}