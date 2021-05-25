using System;
using API.Requests;
using MediatR;

namespace API.Commands.LightBulb
{
    public class CreateLightBulbCommand : IRequest<Models.LightBulb>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public LightBulbRequest Request { get; set; }
    }
}