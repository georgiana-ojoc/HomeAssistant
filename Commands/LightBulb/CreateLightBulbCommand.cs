using System;
using HomeAssistantAPI.Requests;
using MediatR;

namespace HomeAssistantAPI.Commands.LightBulb
{
    public class CreateLightBulbCommand : IRequest<Models.LightBulb>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public LightBulbRequest Request { get; set; }
    }
}