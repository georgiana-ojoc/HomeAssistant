using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.Door
{
    public class GetDoorsQuery : IRequest<IEnumerable<Shared.Models.Door>>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
    }
}