using System.Threading;
using System.Threading.Tasks;
using API.Commands.UserLimitCommand;
using API.Interfaces;
using MediatR;

namespace API.Commands.UserLimit.Handlers
{
    public class PartialUpdateUserLimitCommandHandler : Handler,
        IRequestHandler<PartialUpdateUserLimitCommand, Shared.Models.UserLimit>
    {
        private readonly IUserLimitRepository _repository;

        public PartialUpdateUserLimitCommandHandler(Identity identity, IUserLimitRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.UserLimit> Handle(PartialUpdateUserLimitCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateUserLimitAsync(Identity.Email, request.Id, request.Patch);
        }
    }
}