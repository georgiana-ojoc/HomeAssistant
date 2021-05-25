using System;
using MediatR;

namespace API.Commands.Room
{
    public class DeleteRoomCommand : IRequest<Models.Room>
    {
        public Guid HouseId { get; set; }
        public Guid Id { get; set; }
    }
}