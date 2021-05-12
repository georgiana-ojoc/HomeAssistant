using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.ThermostatCommand.Handlers
{
    public class PartialUpdateThermostatCommandHandler : Handler,
        IRequestHandler<PartialUpdateThermostatCommand, Shared.Models.ThermostatCommand>
    {
        private readonly IThermostatCommandRepository _repository;


        public PartialUpdateThermostatCommandHandler(Identity identity, IThermostatCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.ThermostatCommand> Handle(PartialUpdateThermostatCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateThermostatCommandAsync(Identity.Email, request.ScheduleId,
                request.Id, request.Patch);
        }
    }
}