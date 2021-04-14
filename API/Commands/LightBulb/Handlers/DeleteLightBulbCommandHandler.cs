using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulb.Handlers
{
    public class DeleteLightBulbHandler : IRequestHandler<DeleteLightBulbCommand, Shared.Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public DeleteLightBulbHandler(ILightBulbRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.LightBulb> Handle(DeleteLightBulbCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteLightBulbAsync(request.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}