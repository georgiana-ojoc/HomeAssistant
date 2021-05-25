using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.DoorCommand.Handlers
{
    public class PartialUpdateDoorCommandHandler : Handler,
        IRequestHandler<PartialUpdateDoorCommandCommand, Models.DoorCommand>
    {
        private readonly IDoorCommandRepository _repository;


        public PartialUpdateDoorCommandHandler(Identity identity, IDoorCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.DoorCommand> Handle(PartialUpdateDoorCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateDoorCommandAsync(Identity.Email, request.ScheduleId, request.Id,
                request.Patch);
        }
    }
}