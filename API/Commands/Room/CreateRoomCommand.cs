using System;
using MediatR;
using Shared.Requests;

namespace API.Commands.Room
{
    public class CreateRoomCommand : IRequest<Shared.Models.Room>
    {
        public Guid HouseId { get; set; }
        public RoomRequest Request { get; set; }
    }
}