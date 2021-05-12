using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.ThermostatCommand.Handlers
{
    public class CreateThermostatCommandHandler : Handler, IRequestHandler<CreateThermostatCommand, Shared.Models.ThermostatCommand>
    {
        private readonly IThermostatCommandRepository _repository;

        public CreateThermostatCommandHandler(Identity identity, IThermostatCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.ThermostatCommand> Handle(CreateThermostatCommand request, CancellationToken cancellationToken)
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