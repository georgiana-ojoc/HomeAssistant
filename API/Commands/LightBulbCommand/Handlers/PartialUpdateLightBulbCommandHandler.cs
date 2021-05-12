using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulbCommand.Handlers
{
    public class PartialUpdateLightBulbCommandHandler : Handler,
        IRequestHandler<PartialUpdateLightBulbCommand, Shared.Models.LightBulbCommand>
    {
        private readonly ILightBulbCommandRepository _repository;


        public PartialUpdateLightBulbCommandHandler(Identity identity, ILightBulbCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.LightBulbCommand> Handle(PartialUpdateLightBulbCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateLightBulbCommandAsync(Identity.Email, request.ScheduleId,
                request.Id, request.Patch);
        }
    }
}