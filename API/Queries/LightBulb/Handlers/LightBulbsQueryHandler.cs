using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.LightBulb.Handlers
{
    public class LightBulbsQueryHandler: IRequestHandler<LightBulbsQuery,IEnumerable<Models.LightBulb>>
    {
        private readonly ILightBulbRepository _repository;

        public LightBulbsQueryHandler(ILightBulbRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.LightBulb>> Handle(LightBulbsQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetLightBulbsAsync(request.UserId,
                request.HouseId, request.RoomId);
            return result;
        }
    }
}