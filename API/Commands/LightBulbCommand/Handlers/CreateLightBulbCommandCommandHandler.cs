using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulbCommand.Handlers
{
    public class CreateLightBulbCommandHandler : Handler,
        IRequestHandler<CreateLightBulbCommandCommand, Models.LightBulbCommand>
    {
        private readonly ILightBulbCommandRepository _repository;

        public CreateLightBulbCommandHandler(Identity identity, ILightBulbCommandRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.LightBulbCommand> Handle(CreateLightBulbCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateLightBulbCommandAsync(Identity.Email, request.ScheduleId,
                new Models.LightBulbCommand()
                {
                    LightBulbId = request.Request.LightBulbId,
                    ScheduleId = request.ScheduleId,
                    Color = request.Request.Color,
                    Intensity = request.Request.Intensity
                });
        }
    }
}