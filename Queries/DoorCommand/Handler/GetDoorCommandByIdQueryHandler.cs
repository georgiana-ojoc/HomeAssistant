using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.DoorCommand.Handler
{
    public class GetDoorCommandByIdQueryHandler : HomeAssistantAPI.Handler,
        IRequestHandler<GetDoorCommandByIdQuery, Models.DoorCommand>
    {
        private readonly IDoorCommandRepository _repository;

        public GetDoorCommandByIdQueryHandler(Identity identity, IDoorCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.DoorCommand> Handle(GetDoorCommandByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetDoorCommandByIdAsync(Identity.Email, request.ScheduleId, request.Id);
        }
    }
}