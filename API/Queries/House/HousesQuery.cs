using System.Collections.Generic;
using MediatR;

namespace API.Queries.House
{
    public class HousesQuery : IRequest<IEnumerable<Shared.Models.House>>
    {
        public int Id { get; set; }

        public HousesQuery(int id)
        {
            Id = id;
        }
    }
}