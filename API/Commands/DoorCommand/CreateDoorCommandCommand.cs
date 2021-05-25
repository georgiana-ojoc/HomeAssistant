using System;
using API.Requests;
using MediatR;

namespace API.Commands.DoorCommand
{
    public class CreateDoorCommandCommand : IRequest<Models.DoorCommand>
    {
        public Guid ScheduleId { get; set; }

        public DoorCommandRequest Request { get; set; }
    }
}