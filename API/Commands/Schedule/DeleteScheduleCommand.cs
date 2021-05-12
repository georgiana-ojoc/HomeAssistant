using System;
using MediatR;

namespace API.Commands.Schedule
{
    public class DeleteSchedule : IRequest<Shared.Models.Schedule>
    {
        public Guid Id { get; set; }
    }
}