using System;
using MediatR;

namespace HomeAssistantAPI.Queries.Door
{
    public class GetDoorByIdQuery : IRequest<Models.Door>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
    }
}