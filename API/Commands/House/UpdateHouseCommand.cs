using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Models.Patch;

namespace API.Commands.House
{
    public class UpdateHouseCommand : IRequest<Shared.Models.House>
    {
        public string Email { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<HousePatch> Patch { get; set; }

        public UpdateHouseCommand(string email, Guid id, JsonPatchDocument<HousePatch> patch)
        {
            Email = email;
            Id = id;
            Patch = patch;
        }
    }
}