using System;
using MediatR;

namespace API.Commands.Room
{
    public class DeleteRoomCommand : IRequest<Shared.Models.Room>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid Id { get; set; }

        public DeleteRoomCommand(string email, Guid houseId, Guid id)
        {
            Email = email;
            HouseId = houseId;
            Id = id;
        }
    }
}