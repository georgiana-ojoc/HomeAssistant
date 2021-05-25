using System;
using MediatR;

namespace API.Commands.House
{
    public class DeleteHouseCommand : IRequest<Models.House>
    {
        public Guid Id { get; set; }
    }
}