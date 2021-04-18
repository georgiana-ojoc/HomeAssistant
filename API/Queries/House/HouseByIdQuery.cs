using System;
using MediatR;

namespace API.Queries.House
{
    public class HouseByIdQuery : IRequest<Shared.Models.House>
    {
        public string Email { get; set; }
        public Guid Id { get; set; }

        public HouseByIdQuery(string email, Guid id)
        {
            Email = email;
            Id = id;
        }
    }
}