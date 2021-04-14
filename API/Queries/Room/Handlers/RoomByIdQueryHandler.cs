using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Room.Handlers
{
    public class RoomByIdQueryHandler : IRequestHandler<RoomByIdQuery, Shared.Models.Room>
    {
        private readonly IRoomRepository _repository;

        public RoomByIdQueryHandler(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Room> Handle(RoomByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetRoomByIdAsync(request.Email, request.HouseId, request.Id);
        }
    }
}