using System;
using MediatR;
using Microsoft.AspNet.JsonPatch;
using Shared.Models.Patch;

namespace API.Commands.Thermostat
{
    public class UpdateThermostatCommand : IRequest<Shared.Models.Thermostat>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
        
        public JsonPatchDocument<ThermostatPatch> Patch { get; set; }

        public UpdateThermostatCommand(string email, Guid houseId, Guid roomId, Guid id, 
            JsonPatchDocument<ThermostatPatch> patch)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
            Patch = patch;
        }
    }
}