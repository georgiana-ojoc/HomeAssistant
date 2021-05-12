using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.DoorCommand.Handlers
{
    public class PartialUpdateDoorCommandHandler : Handler,
        IRequestHandler<PartialUpdateDoorCommandCommand, Shared.Models.DoorCommand>
    {
        private readonly IDoorCommandRepository _repository;


        public PartialUpdateDoorCommandHandler(Identity identity, IDoorCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.DoorCommand> Handle(PartialUpdateDoorCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateDoorCommandAsync(Identity.Email, request.ScheduleId, request.Id,
                request.Patch);
        }
    }
}