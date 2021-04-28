using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.LightBulb.Handlers
{
    public class GetLightBulbsQueryHandler : Handler,
        IRequestHandler<GetLightBulbsQuery, IEnumerable<Shared.Models.LightBulb>>
    {
        private readonly ILightBulbRepository _repository;

        public GetLightBulbsQueryHandler(Identity identity, ILightBulbRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Shared.Models.LightBulb>> Handle(GetLightBulbsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetLightBulbsAsync(Identity.Email, request.HouseId, request.RoomId);
        }
    }
}