using System;
using MediatR;

namespace API.Commands.Room
{
    public class AddRoomCommand : IRequest<Shared.Models.Room>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Shared.Models.Room Room { get; set; }

        public AddRoomCommand(string email, Guid houseId, Shared.Models.Room room)
        {
            Email = email;
            HouseId = houseId;
            Room = room;
        }
    }
}