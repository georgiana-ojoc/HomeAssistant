using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Room.Handlers
{
    public class DeleteRoomCommandHandler : Handler, IRequestHandler<DeleteRoomCommand, Models.Room>
    {
        private readonly IRoomRepository _repository;

        public DeleteRoomCommandHandler(Identity identity, IRoomRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Room> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteRoomAsync(Identity.Email, request.HouseId, request.Id);
        }
    }
}