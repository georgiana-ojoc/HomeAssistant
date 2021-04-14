using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulb.Handlers
{
    public class DeleteLightBulbHandler: IRequestHandler<DeleteLightBulb,Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public DeleteLightBulbHandler(ILightBulbRepository repository)
        {
            _repository = repository;
        }

        public async Task<Models.LightBulb> Handle(DeleteLightBulb request, CancellationToken cancellationToken)
        {
            Models.LightBulb lightBulb = await _repository.DeleteLightBulb(request.UserId, request.HouseId,
                request.RoomId, request.Id);
            return lightBulb;
        }
    }
}