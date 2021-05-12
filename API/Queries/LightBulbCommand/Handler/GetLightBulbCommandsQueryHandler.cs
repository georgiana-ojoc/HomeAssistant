using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.LightBulbCommand.Handler
{
    public class GetLightBulbCommandsQueryHandler : API.Handler,
        IRequestHandler<GetLightBulbCommandsQuery, IEnumerable<Shared.Models.LightBulbCommand>>
    {
        private readonly ILightBulbCommandRepository _repository;

        public GetLightBulbCommandsQueryHandler(Identity identity, ILightBulbCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }


        public async Task<IEnumerable<Shared.Models.LightBulbCommand>> Handle(GetLightBulbCommandsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetLightBulbCommandsAsync(Identity.Email, request.ScheduleId);
        }
    }
}