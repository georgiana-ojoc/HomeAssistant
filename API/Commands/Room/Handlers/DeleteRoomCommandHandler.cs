using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Room.Handlers
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Shared.Models.Room>
    {
        private readonly IRoomRepository _repository;

        public DeleteRoomCommandHandler(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Room> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteRoomAsync(request.Email, request.HouseId, request.Id);
        }
    }
}