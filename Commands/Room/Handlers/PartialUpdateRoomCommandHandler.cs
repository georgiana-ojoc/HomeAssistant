using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.Room.Handlers
{
    public class PartialUpdateRoomCommandHandler : Handler,
        IRequestHandler<PartialUpdateRoomCommand, Models.Room>
    {
        private readonly IRoomRepository _repository;

        public PartialUpdateRoomCommandHandler(Identity identity, IRoomRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Room> Handle(PartialUpdateRoomCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateRoomAsync(Identity.Email, request.HouseId, request.Id, request.Patch);
        }
    }
}