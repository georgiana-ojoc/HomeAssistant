using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Thermostat.Handlers
{
    public class ThermostatsHandler : IRequestHandler<ThermostatsQuery, IEnumerable<Shared.Models.Thermostat>>
    {
        private readonly IThermostatRepository _repository;

        public ThermostatsHandler(IThermostatRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Shared.Models.Thermostat>> Handle(ThermostatsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetThermostatsAsync(request.Email, request.HouseId, request.RoomId);
        }
    }
}