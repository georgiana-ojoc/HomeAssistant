using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Room.Handlers
{
    public class RoomByIdHandler : IRequestHandler<RoomById, Shared.Models.Room>
    {
        private readonly IRoomRepository _repository;

        public RoomByIdHandler(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Room> Handle(RoomById request, CancellationToken cancellationToken)
        {
            Shared.Models.Room room = await _repository.GetRoomByIdAsync(request.UserId, request.HouseId, request.Id);
            return room;
        }
    }
}