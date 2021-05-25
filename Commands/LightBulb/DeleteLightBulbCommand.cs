using System;
using MediatR;

namespace HomeAssistantAPI.Commands.LightBulb
{
    public class DeleteLightBulbCommand : IRequest<Models.LightBulb>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }
    }
}