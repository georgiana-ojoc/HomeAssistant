using System;
using MediatR;
using Shared.Requests;

namespace API.Commands.LightBulbCommand
{
    public class CreateLightBulbCommandCommand : IRequest<Shared.Models.LightBulbCommand>
    {
        public Guid ScheduleId { get; set; }

        public LightBulbCommandRequest Request { get; set; }
    }
}