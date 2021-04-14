using System.Collections.Generic;
using MediatR;

namespace API.Queries.House
{
    public class HousesQuery : IRequest<IEnumerable<Shared.Models.House>>
    {
        public string Email { get; set; }

        public HousesQuery(string email)
        {
            Email = email;
        }
    }
}