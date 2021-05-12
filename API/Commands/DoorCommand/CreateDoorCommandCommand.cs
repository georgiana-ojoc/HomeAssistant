using System;
using MediatR;
using Shared.Requests;

namespace API.Commands.DoorCommand
{
    public class CreateDoorCommandCommand : IRequest<Shared.Models.DoorCommand>
    {
        public Guid ScheduleId { get; set; }

        public DoorCommandRequest Request { get; set; }
    }
}