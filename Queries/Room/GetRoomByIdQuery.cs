using System;
using MediatR;

namespace HomeAssistantAPI.Queries.Room
{
    public class GetRoomByIdQuery : IRequest<Models.Room>
    {
        public Guid HouseId { get; set; }
        public Guid Id { get; set; }
    }
}