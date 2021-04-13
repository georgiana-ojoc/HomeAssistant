using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using MediatR;

namespace API.Queries.Handlers
{
    public class DoorByIdHandler: IRequestHandler<DoorById,Door>
    {
        private readonly IDoorRepository _repository;
        public DoorByIdHandler(IDoorRepository repository)
        {
            _repository = repository;
        }
        public async Task<Door> Handle(DoorById request, CancellationToken cancellationToken)
        {
            Door door = await _repository.GetDoorByIdAsync(request.UserId,
                request.HouseId, request.RoomId, request.Id);
            return door;
        }
    }
}