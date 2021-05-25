using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using HomeAssistantAPI.Responses;
using MediatR;

namespace HomeAssistantAPI.Queries.ThermostatCommand.Handler
{
    public class GetThermostatCommandsQueryHandler : HomeAssistantAPI.Handler,
        IRequestHandler<GetThermostatCommandsQuery, IEnumerable<ThermostatCommandResponse>>
    {
        private readonly IThermostatCommandRepository _repository;

        public GetThermostatCommandsQueryHandler(Identity identity, IThermostatCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ThermostatCommandResponse>> Handle(GetThermostatCommandsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetThermostatCommandsAsync(Identity.Email, request.ScheduleId);
        }
    }
}