using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.LightBulb
{
    public class LightBulbsQuery : IRequest<IEnumerable<Shared.Models.LightBulb>>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public LightBulbsQuery(string email, Guid houseId, Guid roomId)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
        }
    }
}