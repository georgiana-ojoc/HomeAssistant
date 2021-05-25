using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.House.Handlers
{
    public class PartialUpdateHouseCommandHandler : Handler,
        IRequestHandler<PartialUpdateHouseCommand, Models.House>
    {
        private readonly IHouseRepository _repository;

        public PartialUpdateHouseCommandHandler(Identity identity, IHouseRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.House> Handle(PartialUpdateHouseCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateHouseAsync(Identity.Email, request.Id, request.Patch);
        }
    }
}