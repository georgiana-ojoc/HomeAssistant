using System;
using MediatR;

namespace API.Queries.Door
{
    public class GetDoorByIdQuery : IRequest<Shared.Models.Door>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
    }
}