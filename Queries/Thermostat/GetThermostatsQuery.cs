using System;
using System.Collections.Generic;
using MediatR;

namespace HomeAssistantAPI.Queries.Thermostat
{
    public class GetThermostatsQuery : IRequest<IEnumerable<Models.Thermostat>>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
    }
}