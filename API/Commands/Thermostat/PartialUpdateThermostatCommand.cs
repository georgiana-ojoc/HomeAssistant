using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Requests;

namespace API.Commands.Thermostat
{
    public class PartialUpdateThermostatCommand : IRequest<Shared.Models.Thermostat>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<ThermostatRequest> Patch { get; set; }
    }
}