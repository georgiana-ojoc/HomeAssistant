using System.Collections.Generic;
using MediatR;

namespace API.Queries.Schedule
{
    public class GetSchedulesQuery : IRequest<IEnumerable<Shared.Models.Schedule>>
    {
    }
}