using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.DoorCommand.Handlers
{
    public class DeleteDoorCommandHandler : Handler, IRequestHandler<DeleteDoorCommand, Shared.Models.DoorCommand>
    {
        private readonly IDoorCommandRepository _repository;

        public DeleteDoorCommandHandler(Identity identity, IDoorCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.DoorCommand> Handle(DeleteDoorCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteDoorCommandAsync(Identity.Email, request.ScheduleId, request.Id);
        }
    }
}