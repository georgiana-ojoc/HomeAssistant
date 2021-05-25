using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulb.Handlers
{
    public class DeleteLightBulbHandler : Handler, IRequestHandler<DeleteLightBulbCommand, Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public DeleteLightBulbHandler(Identity identity, ILightBulbRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.LightBulb> Handle(DeleteLightBulbCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteLightBulbAsync(Identity.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}