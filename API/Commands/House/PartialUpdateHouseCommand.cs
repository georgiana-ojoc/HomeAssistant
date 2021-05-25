using System;
using API.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Commands.House
{
    public class PartialUpdateHouseCommand : IRequest<Models.House>
    {
        public Guid Id { get; set; }

        public JsonPatchDocument<HouseRequest> Patch { get; set; }
    }
}