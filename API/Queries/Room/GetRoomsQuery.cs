using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.Room
{
    public class GetRoomsQuery : IRequest<IEnumerable<Shared.Models.Room>>
    {
        public Guid HouseId { get; set; }
    }
}