using System;
using MediatR;

namespace HomeAssistantAPI.Commands.House
{
    public class DeleteHouseCommand : IRequest<Models.House>
    {
        public Guid Id { get; set; }
    }
}