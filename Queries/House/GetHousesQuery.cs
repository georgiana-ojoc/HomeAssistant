using System.Collections.Generic;
using MediatR;

namespace HomeAssistantAPI.Queries.House
{
    public class GetHousesQuery : IRequest<IEnumerable<Models.House>>
    {
    }
}