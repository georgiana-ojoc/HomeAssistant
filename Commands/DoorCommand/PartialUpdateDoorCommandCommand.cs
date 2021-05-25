using System;
using HomeAssistantAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeAssistantAPI.Commands.DoorCommand
{
    public class PartialUpdateDoorCommandCommand : IRequest<Models.DoorCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<DoorCommandRequest> Patch { get; set; }
    }
}