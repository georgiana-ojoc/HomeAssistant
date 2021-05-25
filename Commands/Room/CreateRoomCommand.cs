using System;
using HomeAssistantAPI.Requests;
using MediatR;

namespace HomeAssistantAPI.Commands.Room
{
    public class CreateRoomCommand : IRequest<Models.Room>
    {
        public Guid HouseId { get; set; }
        public RoomRequest Request { get; set; }
    }
}