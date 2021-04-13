using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using MediatR;

namespace API.Commands.Handlers
{
    public class DeleteDoorHandler: IRequestHandler<DeleteDoor,Door>
    {
        private readonly IDoorRepository _repository;
        public DeleteDoorHandler(IDoorRepository repository)
        {
            _repository = repository;
        }
        public async Task<Door> Handle(DeleteDoor request, CancellationToken cancellationToken)
        {
            Door door = await _repository.DeleteDoor(request.UserId,
                request.HouseId, request.RoomId, request.Id);
            return door;
        }
    }
}