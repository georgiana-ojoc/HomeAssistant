using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Thermostat.Handlers
{
    public class PartialUpdateThermostatCommandHandler : Handler,
        IRequestHandler<PartialUpdateThermostatCommand, Models.Thermostat>
    {
        private readonly IThermostatRepository _repository;


        public PartialUpdateThermostatCommandHandler(Identity identity, IThermostatRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Thermostat> Handle(PartialUpdateThermostatCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateThermostatAsync(Identity.Email, request.HouseId, request.RoomId,
                request.Id, request.Patch);
        }
    }
}