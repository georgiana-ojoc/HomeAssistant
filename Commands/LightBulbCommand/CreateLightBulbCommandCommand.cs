using System;
using HomeAssistantAPI.Requests;
using MediatR;

namespace HomeAssistantAPI.Commands.LightBulbCommand
{
    public class CreateLightBulbCommandCommand : IRequest<Models.LightBulbCommand>
    {
        public Guid ScheduleId { get; set; }

        public LightBulbCommandRequest Request { get; set; }
    }
}