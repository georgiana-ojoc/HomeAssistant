using System;
using MediatR;
using Microsoft.AspNet.JsonPatch;
using Shared.Models.Patch;

namespace API.Commands.Door
{
    public class UpdateDoorCommand: IRequest<Shared.Models.Door>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
        
        public JsonPatchDocument<DoorPatch> Patch { get; set; }

        public UpdateDoorCommand(string email, Guid houseId, Guid roomId, Guid id, 
            JsonPatchDocument<DoorPatch> patch)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
            Patch = patch;
        }
    }
}