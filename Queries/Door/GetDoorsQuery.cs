using System;
using System.Collections.Generic;
using MediatR;

namespace HomeAssistantAPI.Queries.Door
{
    public class GetDoorsQuery : IRequest<IEnumerable<Models.Door>>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
    }
}