using System;
using MediatR;
using Shared.Responses;

namespace API.Queries.LightBulbCommand
{
    public class GetLightBulbCommandByIdQuery : IRequest<LightBulbCommandResponse>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}