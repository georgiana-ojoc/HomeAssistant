using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.LightBulb.Handlers
{
    public class GetLightBulbByIdQueryHandler : Handler, IRequestHandler<GetLightBulbByIdQuery, Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public GetLightBulbByIdQueryHandler(Identity identity, ILightBulbRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.LightBulb> Handle(GetLightBulbByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetLightBulbByIdAsync(Identity.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}