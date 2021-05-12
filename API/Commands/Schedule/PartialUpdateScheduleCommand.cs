using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Requests;

namespace API.Commands.Schedule
{
    public class PartialUpdateScheduleCommand : IRequest<Shared.Models.Schedule>
    {
        public Guid Id { get; set; }

        public JsonPatchDocument<ScheduleRequest> Patch { get; set; }
    }
}