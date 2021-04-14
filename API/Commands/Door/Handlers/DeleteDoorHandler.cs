using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Door.Handlers
{
    public class DeleteDoorHandler : IRequestHandler<DeleteDoor, Shared.Models.Door>
    {
        private readonly IDoorRepository _repository;

        public DeleteDoorHandler(IDoorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Door> Handle(DeleteDoor request, CancellationToken cancellationToken)
        {
            Shared.Models.Door door = await _repository.DeleteDoorAsync(request.UserId,
                request.HouseId, request.RoomId, request.Id);
            return door;
        }
    }
}