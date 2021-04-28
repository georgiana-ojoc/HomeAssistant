using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Requests;

namespace API.Commands.House
{
    public class PartialUpdateHouseCommand : IRequest<Shared.Models.House>
    {
        public Guid Id { get; set; }

        public JsonPatchDocument<HouseRequest> Patch { get; set; }
    }
}