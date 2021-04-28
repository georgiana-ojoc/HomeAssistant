using System.Collections.Generic;
using MediatR;

namespace API.Queries.House
{
    public class GetHousesQuery : IRequest<IEnumerable<Shared.Models.House>>
    {
    }
}