using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.LightBulb.Handlers
{
    public class GetLightBulbsQueryHandler : Handler,
        IRequestHandler<GetLightBulbsQuery, IEnumerable<Models.LightBulb>>
    {
        private readonly ILightBulbRepository _repository;

        public GetLightBulbsQueryHandler(Identity identity, ILightBulbRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.LightBulb>> Handle(GetLightBulbsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetLightBulbsAsync(Identity.Email, request.HouseId, request.RoomId);
        }
    }
}