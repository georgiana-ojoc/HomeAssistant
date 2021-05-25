using System;
using HomeAssistantAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeAssistantAPI.Commands.LightBulbCommand
{
    public class PartialUpdateLightBulbCommandCommand : IRequest<Models.LightBulbCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<LightBulbCommandRequest> Patch { get; set; }
    }
}