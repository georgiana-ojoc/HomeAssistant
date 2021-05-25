using System;
using HomeAssistantAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeAssistantAPI.Commands.Door
{
    public class PartialUpdateDoorCommand : IRequest<Models.Door>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<DoorRequest> Patch { get; set; }
    }
}