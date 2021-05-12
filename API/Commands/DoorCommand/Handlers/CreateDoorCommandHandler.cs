using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.DoorCommand.Handlers
{
    public class CreateDoorCommandHandler : Handler, IRequestHandler<CreateDoorCommand, Shared.Models.DoorCommand>
    {
        private readonly IDoorCommandRepository _repository;

        public CreateDoorCommandHandler(Identity identity, IDoorCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.DoorCommand> Handle(CreateDoorCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateDoorCommandAsync(Identity.Email, request.ScheduleId,
                new Shared.Models.DoorCommand()
                {
                    DoorId = request.Request.DoorId,
                    ScheduleId = request.ScheduleId,
                    Locked = request.Request.Locked
                });
        }
    }
}