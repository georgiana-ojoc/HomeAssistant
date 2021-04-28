using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Thermostat.Handlers
{
    public class CreateThermostatCommandHandler : Handler,
        IRequestHandler<CreateThermostatCommand, Shared.Models.Thermostat>
    {
        private readonly IThermostatRepository _repository;

        public CreateThermostatCommandHandler(Identity identity, IThermostatRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Thermostat> Handle(CreateThermostatCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateThermostatAsync(Identity.Email, request.HouseId, request.RoomId,
                new Shared.Models.Thermostat
                {
                    Name = request.Request.Name,
                    Temperature = request.Request.Temperature
                });
        }
    }
}