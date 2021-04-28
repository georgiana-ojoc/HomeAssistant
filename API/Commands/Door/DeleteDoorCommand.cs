using System;
using MediatR;

namespace API.Commands.Door
{
    public class DeleteDoorCommand : IRequest<Shared.Models.Door>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
    }
}