using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.Door.Handlers
{
    public class PartialUpdateDoorCommandHandler : Handler,
        IRequestHandler<PartialUpdateDoorCommand, Models.Door>
    {
        private readonly IDoorRepository _repository;


        public PartialUpdateDoorCommandHandler(Identity identity, IDoorRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Door> Handle(PartialUpdateDoorCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateDoorAsync(Identity.Email, request.HouseId, request.RoomId,
                request.Id, request.Patch);
        }
    }
}