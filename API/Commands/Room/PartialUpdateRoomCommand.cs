using System;
using API.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Commands.Room
{
    public class PartialUpdateRoomCommand : IRequest<Models.Room>
    {
        public Guid HouseId { get; set; }
        public Guid Id { get; set; }
        public JsonPatchDocument<RoomRequest> Patch { get; set; }
    }
}