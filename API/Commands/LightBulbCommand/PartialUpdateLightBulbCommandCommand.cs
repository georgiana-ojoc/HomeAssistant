using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Requests;

namespace API.Commands.LightBulbCommand
{
    public class PartialUpdateLightBulbCommandCommand : IRequest<Shared.Models.LightBulbCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }

        public JsonPatchDocument<LightBulbCommandRequest> Patch { get; set; }
    }
}