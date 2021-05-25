using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.DoorCommand.Handlers
{
    public class CreateDoorCommandCommandHandler : Handler,
        IRequestHandler<CreateDoorCommandCommand, Models.DoorCommand>
    {
        private readonly IDoorCommandRepository _repository;

        public CreateDoorCommandCommandHandler(Identity identity, IDoorCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.DoorCommand> Handle(CreateDoorCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateDoorCommandAsync(Identity.Email, request.ScheduleId,
                new Models.DoorCommand()
                {
                    DoorId = request.Request.DoorId,
                    ScheduleId = request.ScheduleId,
                    Locked = request.Request.Locked
                });
        }
    }
}