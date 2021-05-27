using System;
using API.Responses;
using MediatR;

namespace API.Queries.DoorCommand
{
    public class GetDoorCommandByIdQuery : IRequest<DoorCommandResponse>
    {
        public Guid ScheduleId { get; set; }
        public Guid Id { get; set; }
    }
}