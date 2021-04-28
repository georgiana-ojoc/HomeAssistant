using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Thermostat.Handlers
{
    public class GetThermostatsHandler : Handler,
        IRequestHandler<GetThermostatsQuery, IEnumerable<Shared.Models.Thermostat>>
    {
        private readonly IThermostatRepository _repository;

        public GetThermostatsHandler(Identity identity, IThermostatRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Shared.Models.Thermostat>> Handle(GetThermostatsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetThermostatsAsync(Identity.Email, request.HouseId, request.RoomId);
        }
    }
}