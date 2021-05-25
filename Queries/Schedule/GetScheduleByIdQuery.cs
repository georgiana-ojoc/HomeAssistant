using System;
using MediatR;

namespace HomeAssistantAPI.Queries.Schedule
{
    public class GetScheduleByIdQuery : IRequest<Models.Schedule>
    {
        public Guid Id { get; set; }
    }
}