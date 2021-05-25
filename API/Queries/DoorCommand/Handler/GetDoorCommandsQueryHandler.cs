using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using API.Responses;
using MediatR;

namespace API.Queries.DoorCommand.Handler
{
    public class GetDoorCommandsQueryHandler : API.Handler,
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