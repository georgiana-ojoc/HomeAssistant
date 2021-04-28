using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Requests;

namespace API.Commands.LightBulb
{
    public class PartialUpdateLightBulbCommand : IRequest<Shared.Models.LightBulb>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<LightBulbRequest> Patch { get; set; }
    }
}