using System;
using MediatR;

namespace API.Queries.DoorCommand
{
    public class GetDoorCommandByIdQuery : IRequest<Shared.Models.DoorCommand>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}