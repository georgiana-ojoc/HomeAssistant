using System;
using MediatR;

namespace API.Queries.Thermostat
{
    public class GetThermostatByIdQuery : IRequest<Models.Thermostat>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
    }
}