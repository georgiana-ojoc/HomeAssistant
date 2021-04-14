using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulb.Handlers
{
    public class AddLightBulbCommandHandler : IRequestHandler<AddLightBulbCommand, Shared.Models.LightBulb>
    {
        private readonly ILightBulbRepository _repository;

        public AddLightBulbCommandHandler(ILightBulbRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.LightBulb> Handle(AddLightBulbCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateLightBulbAsync(request.Email, request.HouseId, request.RoomId,
                request.LightBulb);
        }
    }
}