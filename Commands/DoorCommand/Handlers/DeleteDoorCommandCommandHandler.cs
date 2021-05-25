using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.DoorCommand.Handlers
{
    public class DeleteDoorCommandCommandHandler : Handler,
        IRequestHandler<DeleteDoorCommandCommand, Models.DoorCommand>
    {
        private readonly IDoorCommandRepository _repository;

        public DeleteDoorCommandCommandHandler(Identity identity, IDoorCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.DoorCommand> Handle(DeleteDoorCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteDoorCommandAsync(Identity.Email, request.ScheduleId, request.Id);
        }
    }
}