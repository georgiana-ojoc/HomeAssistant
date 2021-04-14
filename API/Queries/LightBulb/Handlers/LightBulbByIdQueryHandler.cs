using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.LightBulb.Handlers
{
    public class LightBulbByIdQueryHandler : IRequestHandler<LightBulbByIdQuery, Shared.Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public LightBulbByIdQueryHandler(ILightBulbRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.LightBulb> Handle(LightBulbByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetLightBulbByIdAsync(request.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}