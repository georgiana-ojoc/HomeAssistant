using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulb.Handlers
{
    public class AddLightBulbHandler: IRequestHandler<AddLightBulb,Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public AddLightBulbHandler(ILightBulbRepository repository)
        {
            _repository = repository;
        }

        public async Task<Models.LightBulb> Handle(AddLightBulb request, CancellationToken cancellationToken)
        {
            Models.LightBulb lightBulb = await _repository.CreateLightBulb(request.UserId, request.HouseId,
                request.RoomId, request.LightBulb);
            return lightBulb;
        }
    }
}