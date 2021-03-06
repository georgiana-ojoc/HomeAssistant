using System;
using MediatR;

namespace API.Queries.LightBulb
{
    public class GetLightBulbByIdQuery : IRequest<Models.LightBulb>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
    }
}