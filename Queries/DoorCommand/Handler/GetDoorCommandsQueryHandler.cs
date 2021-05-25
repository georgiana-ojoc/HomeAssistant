using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using HomeAssistantAPI.Responses;
using MediatR;

namespace HomeAssistantAPI.Queries.DoorCommand.Handler
{
    public class GetDoorCommandsQueryHandler : HomeAssistantAPI.Handler,
        IRequestHandler<GetDoorCommandsQuery, IEnumerable<DoorCommandResponse>>
    {
        private readonly IDoorCommandRepository _repository;

        public GetDoorCommandsQueryHandler(Identity identity, IDoorCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }


        public async Task<IEnumerable<DoorCommandResponse>> Handle(GetDoorCommandsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetDoorCommandsAsync(Identity.Email, request.ScheduleId);
        }
    }
}