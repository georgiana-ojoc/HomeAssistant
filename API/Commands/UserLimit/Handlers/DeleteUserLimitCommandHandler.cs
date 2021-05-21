using System.Threading;
using System.Threading.Tasks;
using API.Commands.UserLimitCommand;
using API.Interfaces;
using MediatR;

namespace API.Commands.UserLimit.Handlers
{
    public class DeleteUserLimitCommandHandler : Handler, IRequestHandler<DeleteUserLimitCommand, Shared.Models.UserLimit>
    {
        private readonly IUserLimitRepository _repository;

        public DeleteUserLimitCommandHandler(Identity identity, IUserLimitRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.UserLimit> Handle(DeleteUserLimitCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteUserLimitAsync(Identity.Email, request.Id);
        }
    }
}