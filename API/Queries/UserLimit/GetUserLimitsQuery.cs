using System.Collections.Generic;
using MediatR;

namespace API.Queries.UserLimit
{
    public class GetUserLimitsQuery : IRequest<IEnumerable<Shared.Models.UserLimit>>
    {
    }
}