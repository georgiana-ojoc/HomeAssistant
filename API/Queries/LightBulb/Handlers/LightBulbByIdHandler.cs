using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.LightBulb.Handlers
{
    public class LightBulbByIdHandler: IRequestHandler<LightBulbById,Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public LightBulbByIdHandler(ILightBulbRepository repository)
        {
            _repository = repository;
        }

        public async Task<Models.LightBulb> Handle(LightBulbById request, CancellationToken cancellationToken)
        {
            Models.LightBulb lightBulb = await _repository.GetLightBulbByIdAsync(request.UserId,
                request.HouseId, request.RoomId, request.Id);
            return lightBulb;
        }
    }
}