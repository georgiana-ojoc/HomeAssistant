using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Room.Handlers
{
    public class DeleteRoomHandler : IRequestHandler<DeleteRoom, Shared.Models.Room>
    {
        private readonly IRoomRepository _repository;

        public DeleteRoomHandler(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Room> Handle(DeleteRoom request, CancellationToken cancellationToken)
        {
            Shared.Models.Room room = await _repository.DeleteRoomAsync(request.UserId, request.HouseId, request.Id);
            return room;
        }
    }
}