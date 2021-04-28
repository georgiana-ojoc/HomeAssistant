using System;
using MediatR;
using Shared.Requests;

namespace API.Commands.Door
{
    public class CreateDoorCommand : IRequest<Shared.Models.Door>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public DoorRequest Request { get; set; }
    }
}