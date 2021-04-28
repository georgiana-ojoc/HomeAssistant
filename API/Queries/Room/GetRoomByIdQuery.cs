using System;
using MediatR;

namespace API.Queries.Room
{
    public class GetRoomByIdQuery : IRequest<Shared.Models.Room>
    {
        public Guid HouseId { get; set; }
        public Guid Id { get; set; }
    }
}