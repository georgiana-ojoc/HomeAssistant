using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.Door.Handlers
{
    public class DeleteDoorCommandHandler : Handler, IRequestHandler<DeleteDoorCommand, Models.Door>
    {
        private readonly IDoorRepository _repository;

        public DeleteDoorCommandHandler(Identity identity, IDoorRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Door> Handle(DeleteDoorCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteDoorAsync(Identity.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}