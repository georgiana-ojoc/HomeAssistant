using System;
using MediatR;
using Microsoft.AspNet.JsonPatch;
using Shared.Models.Patch;

namespace API.Commands.Room
{
    public class UpdateRoomCommand: IRequest<Shared.Models.Room>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid Id { get; set; }
        public JsonPatchDocument<RoomPatch> Patch { get; set; }

        public UpdateRoomCommand(string email, Guid houseId, Guid id, JsonPatchDocument<RoomPatch> patch)
        {
            Email = email;
            HouseId = houseId;
            Id = id;
            Patch = patch;
        }
    }
}