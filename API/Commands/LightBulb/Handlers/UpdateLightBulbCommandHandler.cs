using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;
using MediatR;
using Shared.Models.Patch;

namespace API.Commands.LightBulb.Handlers
{
    public class UpdateLightBulbCommandHandler : IRequestHandler<UpdateLightBulbCommand,Shared.Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;
        private readonly IMapper _mapper;

        public UpdateLightBulbCommandHandler(ILightBulbRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Shared.Models.LightBulb> Handle(UpdateLightBulbCommand request, 
            CancellationToken cancellationToken)
        {
            Shared.Models.LightBulb lightBulb = await _repository.GetLightBulbByIdAsync(request.Email, request.HouseId,
                request.RoomId,request.Id);
            if (lightBulb == null)
            {
                return null;
            }

            LightBulbPatch lightBulbToPatch = _mapper.Map<LightBulbPatch>(lightBulb);
            request.Patch.ApplyTo(lightBulbToPatch);
            _mapper.Map(lightBulbToPatch, lightBulb);
            

            if (!await _repository.SaveChangesAsync()) {
                return null;
            }
            return lightBulb;
        }
    }
}