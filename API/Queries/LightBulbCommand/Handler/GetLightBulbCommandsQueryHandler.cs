using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using API.Responses;
using MediatR;

namespace API.Queries.LightBulbCommand.Handler
{
    public class GetLightBulbCommandsQueryHandler : API.Handler,
        IRequestHandler<GetLightBulbCommandsQuery, IEnumerable<LightBulbCommandResponse>>
    {
        private readonly ILightBulbCommandRepository _repository;

        public GetLightBulbCommandsQueryHandler(Identity identity, ILightBulbCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }


        public async Task<IEnumerable<LightBulbCommandResponse>> Handle(GetLightBulbCommandsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetLightBulbCommandsAsync(Identity.Email, request.ScheduleId);
        }
    }
}