using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.ThermostatCommand.Handlers
{
    public class DeleteThermostatCommandCommandHandler : Handler,
        IRequestHandler<DeleteThermostatCommandCommand, Models.ThermostatCommand>
    {
        private readonly IThermostatCommandRepository _repository;

        public DeleteThermostatCommandCommandHandler(Identity identity, IThermostatCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.ThermostatCommand> Handle(DeleteThermostatCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteThermostatCommandAsync(Identity.Email, request.ScheduleId, request.Id);
        }
    }
}