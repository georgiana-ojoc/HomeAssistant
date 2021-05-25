using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.ThermostatCommand.Handler
{
    public class GetThermostatCommandByIdQueryHandler : HomeAssistantAPI.Handler,
        IRequestHandler<GetThermostatCommandByIdQuery, Models.ThermostatCommand>
    {
        private readonly IThermostatCommandRepository _repository;

        public GetThermostatCommandByIdQueryHandler(Identity identity, IThermostatCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.ThermostatCommand> Handle(GetThermostatCommandByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetThermostatCommandByIdAsync(Identity.Email, request.ScheduleId, request.Id);
        }
    }
}