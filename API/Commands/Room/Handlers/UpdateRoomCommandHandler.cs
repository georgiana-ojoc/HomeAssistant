using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;
using MediatR;
using Shared.Models.Patch;

namespace API.Commands.Room.Handlers
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Shared.Models.Room>
    {
        private readonly IRoomRepository _repository;
        private readonly IMapper _mapper;

        public UpdateRoomCommandHandler(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Shared.Models.Room> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            Shared.Models.Room room = await _repository.GetRoomByIdAsync(request.Email, request.HouseId, request.Id);
            if (room == null)
            {
                return null;
            }

            RoomPatch roomToPatch = _mapper.Map<RoomPatch>(room);
            request.Patch.ApplyTo(roomToPatch);
            _mapper.Map(roomToPatch, room);

            if (!await _repository.SaveChangesAsync())
            {
                return null;
            }

            return room;
        }
    }
}