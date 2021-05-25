using System;
using API.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Commands.Schedule
{
    public class PartialUpdateScheduleCommand : IRequest<Models.Schedule>
    {
        public Guid Id { get; set; }

        public JsonPatchDocument<ScheduleRequest> Patch { get; set; }
    }
}