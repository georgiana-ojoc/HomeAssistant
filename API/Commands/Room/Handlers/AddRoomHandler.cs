using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Room.Handlers
{
    public class AddRoomHandler: IRequestHandler<AddRoom,Models.Room>
    {
        private readonly IRoomRepository _repository;

        public AddRoomHandler(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<Models.Room> Handle(AddRoom request, CancellationToken cancellationToken)
        {
            Models.Room room = await _repository.CreateRoom(request.UserId, request.HouseId, request.Room);
            return room;
        }
    }
}