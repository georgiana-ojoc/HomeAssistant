using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.LightBulb
{
    public class GetLightBulbsQuery : IRequest<IEnumerable<Shared.Models.LightBulb>>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
    }
}