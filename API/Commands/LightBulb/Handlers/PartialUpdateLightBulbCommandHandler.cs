using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulb.Handlers
{
    public class PartialUpdateLightBulbCommandHandler : Handler,
        IRequestHandler<PartialUpdateLightBulbCommand, Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public PartialUpdateLightBulbCommandHandler(Identity identity, ILightBulbRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.LightBulb> Handle(PartialUpdateLightBulbCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateLightBulbAsync(Identity.Email, request.HouseId, request.RoomId,
                request.Id, request.Patch);
        }
    }
}