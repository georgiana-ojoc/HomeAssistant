using System;
using MediatR;

namespace API.Commands.Door
{
    public class DeleteDoorCommand : IRequest<Shared.Models.Door>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public DeleteDoorCommand(string email, Guid houseId, Guid roomId, Guid id)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}