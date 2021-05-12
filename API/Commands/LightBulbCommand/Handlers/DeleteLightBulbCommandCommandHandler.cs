using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.LightBulbCommand.Handlers
{
    public class DeleteLightBulbCommandCommandHandler : Handler,
        IRequestHandler<DeleteLightBulbCommandCommand, Shared.Models.LightBulbCommand>
    {
        private readonly ILightBulbCommandRepository _repository;

        public DeleteLightBulbCommandCommandHandler(Identity identity, ILightBulbCommandRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.LightBulbCommand> Handle(DeleteLightBulbCommandCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteLightBulbCommandAsync(Identity.Email, request.ScheduleId, request.Id);
        }
    }
}