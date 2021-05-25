using API.Requests;
using MediatR;

namespace API.Commands.Schedule
{
    public class CreateScheduleCommand : IRequest<Models.Schedule>
    {
        public ScheduleRequest Request { get; set; }
    }
}