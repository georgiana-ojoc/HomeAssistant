using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Requests;

namespace API.Commands.Room
{
    public class PartialUpdateRoomCommand : IRequest<Shared.Models.Room>
    {
        public Guid HouseId { get; set; }
        public Guid Id { get; set; }
        public JsonPatchDocument<RoomRequest> Patch { get; set; }
    }
}