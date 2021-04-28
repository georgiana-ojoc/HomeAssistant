using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Thermostat.Handlers
{
    public class GetThermostatByIdQueryHandler : Handler,
        IRequestHandler<GetThermostatByIdQuery, Shared.Models.Thermostat>
    {
        private readonly IThermostatRepository _repository;

        public GetThermostatByIdQueryHandler(Identity identity, IThermostatRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Thermostat> Handle(GetThermostatByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetThermostatByIdAsync(Identity.Email, request.HouseId, request.RoomId,
                request.Id);
        }
    }
}