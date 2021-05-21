using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.UserLimit.Handlers
{
    public class GetUserLimitByIdQueryHandler : Handler, IRequestHandler<GetUserLimitByIdQuery, Shared.Models.UserLimit>
    {
        private readonly IUserLimitRepository _repository;

        public GetUserLimitByIdQueryHandler(Identity identity, IUserLimitRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.UserLimit> Handle(GetUserLimitByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetUserLimitByIdAsync(Identity.Email, request.Id);
        }
    }
}