using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Door.Handlers
{
    public class DoorByIdHandler : IRequestHandler<DoorById, Shared.Models.Door>
    {
        private readonly IDoorRepository _repository;

        public DoorByIdHandler(IDoorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Door> Handle(DoorById request, CancellationToken cancellationToken)
        {
            Shared.Models.Door door = await _repository.GetDoorByIdAsync(request.UserId,
                request.HouseId, request.RoomId, request.Id);
            return door;
        }
    }
}