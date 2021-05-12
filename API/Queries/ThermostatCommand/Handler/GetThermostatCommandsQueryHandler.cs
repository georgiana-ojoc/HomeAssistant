using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace API.Queries.ThermostatCommand.Handler
{
    public class GetThermostatCommandsQueryHandler:API.Handler,IRequestHandler<GetThermostatCommandsQuery,IEnumerable<Shared.Models.ThermostatCommand>>
    {
        private readonly IThermostatCommandRepository _repository;

        public GetThermostatCommandsQueryHandler(Identity identity, IThermostatCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Shared.Models.ThermostatCommand>> Handle(GetThermostatCommandsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetDoorCommandByIdAsync(Identity.Email, request.ScheduleId);
        }
        
    }
}