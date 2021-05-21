using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.UserLimit.Handlers
{
    public class GetUserLimitsQueryHandler : Handler, IRequestHandler<GetUserLimitsQuery, IEnumerable<Shared.Models.UserLimit>>
    {
        private readonly IUserLimitRepository _repository;

        public GetUserLimitsQueryHandler(Identity identity, IUserLimitRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Shared.Models.UserLimit>> Handle(GetUserLimitsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetUserLimitsAsync(Identity.Email);
        }
    }
}