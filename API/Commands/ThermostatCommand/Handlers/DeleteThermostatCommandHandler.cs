using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.ThermostatCommand.Handlers
{
    public class DeleteThermostatCommandHandler : Handler,
        IRequestHandler<DeleteThermostatCommand, Shared.Models.ThermostatCommand>
    {
        private readonly IThermostatCommandRepository _repository;

        public DeleteThermostatCommandHandler(Identity identity, IThermostatCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.ThermostatCommand> Handle(DeleteThermostatCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteThermostatCommandAsync(Identity.Email, request.ScheduleId, request.Id);
        }
    }
}