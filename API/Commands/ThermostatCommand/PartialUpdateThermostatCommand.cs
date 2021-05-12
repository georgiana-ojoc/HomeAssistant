using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Requests;

namespace API.Commands.ThermostatCommand
{
    public class PartialUpdateThermostatCommand : IRequest<Shared.Models.ThermostatCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<ThermostatCommandRequest> Patch { get; set; }
    }
}