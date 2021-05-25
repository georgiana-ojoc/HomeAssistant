using System;
using HomeAssistantAPI.Requests;
using MediatR;

namespace HomeAssistantAPI.Commands.DoorCommand
{
    public class CreateDoorCommandCommand : IRequest<Models.DoorCommand>
    {
        public Guid ScheduleId { get; set; }

        public DoorCommandRequest Request { get; set; }
    }
}