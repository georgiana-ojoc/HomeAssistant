using System;
using API.Requests;
using MediatR;

namespace API.Commands.Room
{
    public class CreateRoomCommand : IRequest<Models.Room>
    {
        public Guid HouseId { get; set; }
        public RoomRequest Request { get; set; }
    }
}