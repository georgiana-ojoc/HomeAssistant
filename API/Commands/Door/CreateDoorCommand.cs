using System;
using API.Requests;
using MediatR;

namespace API.Commands.Door
{
    public class CreateDoorCommand : IRequest<Models.Door>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public DoorRequest Request { get; set; }
    }
}