using System.Collections.Generic;
using MediatR;

namespace HomeAssistantAPI.Queries.Schedule
{
    public class GetSchedulesQuery : IRequest<IEnumerable<Models.Schedule>>
    {
    }
}