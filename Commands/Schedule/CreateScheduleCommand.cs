using HomeAssistantAPI.Requests;
using MediatR;

namespace HomeAssistantAPI.Commands.Schedule
{
    public class CreateScheduleCommand : IRequest<Models.Schedule>
    {
        public ScheduleRequest Request { get; set; }
    }
}