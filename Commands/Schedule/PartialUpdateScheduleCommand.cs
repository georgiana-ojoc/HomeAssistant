using System;
using HomeAssistantAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeAssistantAPI.Commands.Schedule
{
    public class PartialUpdateScheduleCommand : IRequest<Models.Schedule>
    {
        public Guid Id { get; set; }

        public JsonPatchDocument<ScheduleRequest> Patch { get; set; }
    }
}