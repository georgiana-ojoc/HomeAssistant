using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.LightBulbCommand.Handler
{
    public class GetLightBulbCommandByIdQueryHandler : HomeAssistantAPI.Handler,
        IRequestHandler<GetLightBulbCommandByIdQuery, Models.LightBulbCommand>
    {
        private readonly ILightBulbCommandRepository _repository;

        public GetLightBulbCommandByIdQueryHandler(Identity identity, ILightBulbCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.LightBulbCommand> Handle(GetLightBulbCommandByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetLightBulbCommandByIdAsync(Identity.Email, request.ScheduleId, request.Id);
        }
    }
}