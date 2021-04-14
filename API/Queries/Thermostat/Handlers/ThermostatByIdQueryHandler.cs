using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Thermostat.Handlers
{
    public class ThermostatByIdQueryHandler : IRequestHandler<ThermostatByIdQuery, Shared.Models.Thermostat>
    {
        private readonly IThermostatRepository _repository;

        public ThermostatByIdQueryHandler(IThermostatRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Thermostat> Handle(ThermostatByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetThermostatByIdAsync(request.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}