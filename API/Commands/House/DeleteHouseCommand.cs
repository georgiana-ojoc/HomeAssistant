using System;
using MediatR;

namespace API.Commands.House
{
    public class DeleteHouseCommand : IRequest<Shared.Models.House>
    {
        public string Email { get; set; }
        public Guid Id { get; set; }

        public DeleteHouseCommand(string email, Guid id)
        {
            Email = email;
            Id = id;
        }
    }
}