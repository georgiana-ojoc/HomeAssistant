using System;
using API.Responses;
using MediatR;

namespace API.Queries.LightBulbCommand
{
    public class GetLightBulbCommandByIdQuery : IRequest<LightBulbCommandResponse>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}