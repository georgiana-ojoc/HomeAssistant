using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.ThermostatCommand.Handlers
{
    public class PartialUpdateThermostatCommandCommandHandler : Handler,
        IRequestHandler<PartialUpdateThermostatCommandCommand, Models.ThermostatCommand>
    {
        private readonly IThermostatCommandRepository _repository;


        public PartialUpdateThermostatCommandCommandHandler(Identity identity, IThermostatCommandRepository repository)
            :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.ThermostatCommand> Handle(PartialUpdateThermostatCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateThermostatCommandAsync(Identity.Email, request.ScheduleId,
                request.Id, request.Patch);
        }
    }
}