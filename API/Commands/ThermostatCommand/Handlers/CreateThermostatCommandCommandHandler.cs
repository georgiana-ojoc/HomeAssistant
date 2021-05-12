using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.ThermostatCommand.Handlers
{
    public class CreateThermostatCommandCommandHandler : Handler,
        IRequestHandler<CreateThermostatCommandCommand, Shared.Models.ThermostatCommand>
    {
        private readonly IThermostatCommandRepository _repository;

        public CreateThermostatCommandCommandHandler(Identity identity, IThermostatCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.ThermostatCommand> Handle(CreateThermostatCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateThermostatCommandAsync(Identity.Email, request.ScheduleId,
                new Shared.Models.ThermostatCommand()
                {
                    ThermostatId = request.Request.ThermostatId,
                    ScheduleId = request.ScheduleId,
                    Temperature = request.Request.Temperature
                });
        }
    }
}