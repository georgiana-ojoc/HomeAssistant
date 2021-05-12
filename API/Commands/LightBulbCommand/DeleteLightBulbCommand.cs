using System;
using MediatR;

namespace API.Commands.LightBulbCommand
{
    public class DeleteLightBulbCommand : IRequest<Shared.Models.LightBulbCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}