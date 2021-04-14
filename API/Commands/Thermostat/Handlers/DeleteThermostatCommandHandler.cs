using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Thermostat.Handlers
{
    public class DeleteThermostatCommandHandler : IRequestHandler<DeleteThermostatCommand, Shared.Models.Thermostat>
    {
        private readonly IThermostatRepository _repository;

        public DeleteThermostatCommandHandler(IThermostatRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Thermostat> Handle(DeleteThermostatCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteThermostatAsync(request.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}