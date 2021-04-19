using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Models.Patch;

namespace API.Commands.LightBulb
{
    public class UpdateLightBulbCommand : IRequest<Shared.Models.LightBulb>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<LightBulbPatch> Patch { get; set; }

        public UpdateLightBulbCommand(string email, Guid houseId, Guid roomId, Guid id,
            JsonPatchDocument<LightBulbPatch> patch)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
            Patch = patch;
        }
    }
}