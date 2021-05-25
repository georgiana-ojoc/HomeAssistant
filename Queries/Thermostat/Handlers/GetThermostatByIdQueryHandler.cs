using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.Thermostat.Handlers
{
    public class GetThermostatByIdQueryHandler : Handler,
        IRequestHandler<GetThermostatByIdQuery, Models.Thermostat>
    {
        private readonly IThermostatRepository _repository;

        public GetThermostatByIdQueryHandler(Identity identity, IThermostatRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Thermostat> Handle(GetThermostatByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetThermostatByIdAsync(Identity.Email, request.HouseId, request.RoomId,
                request.Id);
        }
    }
}