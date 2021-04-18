using System;
using MediatR;

namespace API.Commands.Door
{
    public class AddDoorCommand : IRequest<Shared.Models.Door>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }

        public Shared.Models.Door Door { get; set; }

        public AddDoorCommand(string email, Guid houseId, Guid roomId, Shared.Models.Door door)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Door = door;
        }
    }
}