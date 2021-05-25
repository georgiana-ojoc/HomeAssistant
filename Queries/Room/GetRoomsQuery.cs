using System;
using System.Collections.Generic;
using MediatR;

namespace HomeAssistantAPI.Queries.Room
{
    public class GetRoomsQuery : IRequest<IEnumerable<Models.Room>>
    {
        public Guid HouseId { get; set; }
    }
}