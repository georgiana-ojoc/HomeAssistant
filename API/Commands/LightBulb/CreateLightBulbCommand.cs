using System;
using MediatR;
using Shared.Requests;

namespace API.Commands.LightBulb
{
    public class CreateLightBulbCommand : IRequest<Shared.Models.LightBulb>
    {
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public LightBulbRequest Request { get; set; }
    }
}