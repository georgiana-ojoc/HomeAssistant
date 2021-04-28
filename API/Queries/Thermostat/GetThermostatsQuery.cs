using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.Thermostat
{
    public class GetThermostatsQuery : IRequest<IEnumerable<Shared.Models.Thermostat>>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
    }
}