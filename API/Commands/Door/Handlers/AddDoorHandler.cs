using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Door.Handlers
{
    public class AddDoorHandler : IRequestHandler<AddDoor, Shared.Models.Door>
    {
        private readonly IDoorRepository _repository;

        public AddDoorHandler(IDoorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Door> Handle(AddDoor request, CancellationToken cancellationToken)
        {
            Shared.Models.Door door = await _repository.CreateDoorAsync(request.UserId,
                request.HouseId, request.RoomId, request.Door);
            return door;
        }
    }
}