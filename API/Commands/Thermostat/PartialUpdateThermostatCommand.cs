using System;
using API.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Commands.Thermostat
{
    public class PartialUpdateThermostatCommand : IRequest<Models.Thermostat>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<ThermostatRequest> Patch { get; set; }
    }
}