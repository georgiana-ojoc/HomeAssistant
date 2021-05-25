using System;
using MediatR;

namespace HomeAssistantAPI.Commands.Schedule
{
    public class DeleteScheduleCommand : IRequest<Models.Schedule>
    {
        public Guid Id { get; set; }
    }
}