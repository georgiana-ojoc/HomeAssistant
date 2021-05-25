using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.Thermostat.Handlers
{
    public class CreateThermostatCommandHandler : Handler,
        IRequestHandler<CreateThermostatCommand, Models.Thermostat>
    {
        private readonly IThermostatRepository _repository;

        public CreateThermostatCommandHandler(Identity identity, IThermostatRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Thermostat> Handle(CreateThermostatCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateThermostatAsync(Identity.Email, request.HouseId, request.RoomId,
                new Models.Thermostat
                {
                    Name = request.Request.Name,
                    Temperature = request.Request.Temperature
                });
        }
    }
}