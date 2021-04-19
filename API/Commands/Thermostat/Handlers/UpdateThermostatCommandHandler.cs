using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;
using MediatR;
using Shared.Models.Patch;

namespace API.Commands.Thermostat.Handlers
{
    public class UpdateThermostatCommandHandler : IRequestHandler<UpdateThermostatCommand,Shared.Models.Thermostat>
    {
        private readonly IThermostatRepository _repository;
        private readonly IMapper _mapper;

        public UpdateThermostatCommandHandler(IThermostatRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Shared.Models.Thermostat> Handle(UpdateThermostatCommand request, 
            CancellationToken cancellationToken)
        {
            Shared.Models.Thermostat thermostat = await _repository.GetThermostatByIdAsync(request.Email, request.HouseId,
                request.RoomId,request.Id);
            if (thermostat== null)
            {
                return null;
            }

            ThermostatPatch thermostatToPatch = _mapper.Map<ThermostatPatch>(thermostat);
            request.Patch.ApplyTo(thermostatToPatch);
            _mapper.Map(thermostatToPatch, thermostat);
            

            if (!await _repository.SaveChangesAsync()) {
                return null;
            }
            return thermostat;
        }
    }
}