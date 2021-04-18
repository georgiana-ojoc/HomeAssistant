using System;
using MediatR;

namespace API.Commands.LightBulb
{
    public class AddLightBulbCommand : IRequest<Shared.Models.LightBulb>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public Shared.Models.LightBulb LightBulb { get; set; }

        public AddLightBulbCommand(string email, Guid houseId, Guid roomId, Shared.Models.LightBulb lightBulb)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            LightBulb = lightBulb;
        }
    }
}