using System;
using HomeAssistantAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeAssistantAPI.Commands.LightBulb
{
    public class PartialUpdateLightBulbCommand : IRequest<Models.LightBulb>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<LightBulbRequest> Patch { get; set; }
    }
}