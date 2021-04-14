using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Room.Handlers
{
    public class AddRoomHandler : IRequestHandler<AddRoom, Shared.Models.Room>
    {
        private readonly IRoomRepository _repository;

        public AddRoomHandler(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Room> Handle(AddRoom request, CancellationToken cancellationToken)
        {
            Shared.Models.Room room = await _repository.CreateRoomAsync(request.UserId, request.HouseId, request.Room);
            return room;
        }
    }
}