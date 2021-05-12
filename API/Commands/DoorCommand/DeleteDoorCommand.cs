using System;
using MediatR;

namespace API.Commands.DoorCommand
{
    public class DeleteDoorCommand : IRequest<Shared.Models.DoorCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}