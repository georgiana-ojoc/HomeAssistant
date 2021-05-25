using System;
using MediatR;

namespace API.Commands.LightBulbCommand
{
    public class DeleteLightBulbCommandCommand : IRequest<Models.LightBulbCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}