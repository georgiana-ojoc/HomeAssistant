using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;
using MediatR;
using Shared.Models.Patch;

namespace API.Commands.Door.Handlers
{
    public class UpdateDoorCommandHandler: IRequestHandler<UpdateDoorCommand,Shared.Models.Door>
    {
        private readonly IDoorRepository _repository;
        private readonly IMapper _mapper;

        public UpdateDoorCommandHandler(IDoorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Shared.Models.Door> Handle(UpdateDoorCommand request, CancellationToken cancellationToken)
        {
            Shared.Models.Door door = await _repository.GetDoorByIdAsync(request.Email, request.HouseId,
                request.RoomId,request.Id);
            if (door == null)
            {
                return null;
            }

            DoorPatch doorToPatch = _mapper.Map<DoorPatch>(door);
            request.Patch.ApplyTo(doorToPatch);
            _mapper.Map(doorToPatch, door);
            

            if (!await _repository.SaveChangesAsync()) {
                return null;
            }
            return door;
            
        }
    }
}