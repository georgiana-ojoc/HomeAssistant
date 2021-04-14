using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Thermostat.Handlers
{
    public class AddThermostatCommandHandler : IRequestHandler<AddThermostatCommand, Shared.Models.Thermostat>
    {
        private readonly IThermostatRepository _repository;

        public AddThermostatCommandHandler(IThermostatRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Thermostat> Handle(AddThermostatCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateThermostatAsync(request.Email, request.HouseId, request.RoomId,
                request.Thermostat);
        }
    }
}