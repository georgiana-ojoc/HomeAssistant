using System;
using MediatR;

namespace API.Commands.LightBulb
{
    public class DeleteLightBulbCommand : IRequest<Shared.Models.LightBulb>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public DeleteLightBulbCommand(string email, Guid houseId, Guid roomId, Guid id)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}