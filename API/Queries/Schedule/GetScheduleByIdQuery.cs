using System;
using MediatR;

namespace API.Queries.Schedule
{
    public class GetScheduleByIdQuery : IRequest<Models.Schedule>
    {
        public Guid Id { get; set; }
    }
}