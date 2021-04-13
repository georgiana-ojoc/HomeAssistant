using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using MediatR;

namespace API.Commands.Handlers
{
    public class AddDoorHandler: IRequestHandler<AddDoor,Door>
    {
        private readonly IDoorRepository _repository;
        public AddDoorHandler(IDoorRepository repository)
        {
            _repository = repository;
        }
        public async Task<Door> Handle(AddDoor request, CancellationToken cancellationToken)
        {
            Door door = await _repository.CreateDoor(request.UserId,
                request.HouseId,request.RoomId,request.Door);
            return door;
        }
    }
}