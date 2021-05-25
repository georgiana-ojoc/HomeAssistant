using System;
using API.Requests;
using MediatR;

namespace API.Commands.LightBulbCommand
{
    public class CreateLightBulbCommandCommand : IRequest<Models.LightBulbCommand>
    {
        public Guid ScheduleId { get; set; }

        public LightBulbCommandRequest Request { get; set; }
    }
}