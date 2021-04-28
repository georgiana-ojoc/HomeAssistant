using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Requests;

namespace API.Commands.Door
{
    public class PartialUpdateDoorCommand : IRequest<Shared.Models.Door>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<DoorRequest> Patch { get; set; }
    }
}