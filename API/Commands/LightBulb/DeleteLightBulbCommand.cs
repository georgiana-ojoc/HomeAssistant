using System;
using MediatR;

namespace API.Commands.LightBulb
{
    public class DeleteLightBulbCommand : IRequest<Shared.Models.LightBulb>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
    }
}