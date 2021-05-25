using System;
using MediatR;

namespace API.Commands.Schedule
{
    public class DeleteScheduleCommand : IRequest<Models.Schedule>
    {
        public Guid Id { get; set; }
    }
}