using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Thermostat.Handlers
{
    public class DeleteThermostatCommandHandler : Handler,
        IRequestHandler<DeleteThermostatCommand, Models.Thermostat>
    {
        private readonly IThermostatRepository _repository;

        public DeleteThermostatCommandHandler(Identity identity, IThermostatRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Thermostat> Handle(DeleteThermostatCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteThermostatAsync(Identity.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}