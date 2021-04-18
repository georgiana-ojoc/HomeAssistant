using System;
using System.Collections.Generic;
using MediatR;

namespace API.Queries.Room
{
    public class RoomsQuery : IRequest<IEnumerable<Shared.Models.Room>>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }

        public RoomsQuery(string email, Guid houseId)
        {
            Email = email;
            HouseId = houseId;
        }
    }
}