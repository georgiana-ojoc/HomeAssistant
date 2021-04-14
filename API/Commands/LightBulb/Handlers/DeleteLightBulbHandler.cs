using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulb.Handlers
{
    public class DeleteLightBulbHandler: IRequestHandler<DeleteLightBulb,Shared.Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public DeleteLightBulbHandler(ILightBulbRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.LightBulb> Handle(DeleteLightBulb request, CancellationToken cancellationToken)
        {
            Shared.Models.LightBulb lightBulb = await _repository.DeleteLightBulbAsync(request.UserId, request.HouseId,
                request.RoomId, request.Id);
            return lightBulb;
        }
    }
}