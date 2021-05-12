using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Requests;

namespace API.Commands.DoorCommand
{
    public class PartialUpdateDoorCommandCommand : IRequest<Shared.Models.DoorCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<DoorCommandRequest> Patch { get; set; }
    }
}