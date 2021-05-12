using MediatR;
using Shared.Requests;

namespace API.Commands.Schedule
{
    public class CreateScheduleCommand : IRequest<Shared.Models.Schedule>
    {
        public ScheduleRequest Request { get; set; }
    }
}