using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.LightBulbCommand.Handlers
{
    public class PartialUpdateLightBulbCommandCommandHandler : Handler,
        IRequestHandler<PartialUpdateLightBulbCommandCommand, Models.LightBulbCommand>
    {
        private readonly ILightBulbCommandRepository _repository;


        public PartialUpdateLightBulbCommandCommandHandler(Identity identity, ILightBulbCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.LightBulbCommand> Handle(PartialUpdateLightBulbCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateLightBulbCommandAsync(Identity.Email, request.ScheduleId,
                request.Id, request.Patch);
        }
    }
}