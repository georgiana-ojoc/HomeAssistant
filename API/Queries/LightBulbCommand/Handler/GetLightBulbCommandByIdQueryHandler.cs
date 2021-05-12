using System.Threading;
using System.Threading.Tasks;
using API.Queries.LightBulbCommand;
using MediatR;

namespace API.Queries.LightBulbCommand.Handler
{
    public class GetLightBulbCommandByIdQueryHandler:API.Handler,IRequestHandler<GetLightBulbCommandByIdQuery,Shared.Models.LightBulbCommand>
    {
        private readonly ILightBulbCommandRepository _repository;

        public GetLightBulbCommandByIdQueryHandler(Identity identity, ILightBulbCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.LightBulbCommand> Handle(GetLightBulbCommandByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetLightBulbCommandByIdAsync(Identity.Email, request.ScheduleId,request.Id);
        }
    }
}