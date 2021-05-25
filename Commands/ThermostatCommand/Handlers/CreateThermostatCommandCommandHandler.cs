using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.ThermostatCommand.Handlers
{
    public class CreateThermostatCommandCommandHandler : Handler,
        IRequestHandler<CreateThermostatCommandCommand, Models.ThermostatCommand>
    {
        private readonly IThermostatCommandRepository _repository;

        public CreateThermostatCommandCommandHandler(Identity identity, IThermostatCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.ThermostatCommand> Handle(CreateThermostatCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateThermostatCommandAsync(Identity.Email, request.ScheduleId,
                new Models.ThermostatCommand()
                {
                    ThermostatId = request.Request.ThermostatId,
                    ScheduleId = request.ScheduleId,
                    Temperature = request.Request.Temperature
                });
        }
    }
}