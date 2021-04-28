using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulb.Handlers
{
    public class CreateLightBulbCommandHandler : Handler,
        IRequestHandler<CreateLightBulbCommand, Shared.Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public CreateLightBulbCommandHandler(Identity identity, ILightBulbRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.LightBulb> Handle(CreateLightBulbCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateLightBulbAsync(Identity.Email, request.HouseId, request.RoomId,
                new Shared.Models.LightBulb
                {
                    Name = request.Request.Name,
                    Color = request.Request.Color,
                    Intensity = request.Request.Intensity
                });
        }
    }
}