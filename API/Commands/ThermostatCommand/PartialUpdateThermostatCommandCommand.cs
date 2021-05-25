using System;
using API.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Commands.ThermostatCommand
{
    public class PartialUpdateThermostatCommandCommand : IRequest<Models.ThermostatCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<ThermostatCommandRequest> Patch { get; set; }
    }
}